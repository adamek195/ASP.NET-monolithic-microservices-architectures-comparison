using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multimedia.Web.Exceptions.Filters;
using Multimedia.Web.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Multimedia.Web.Services.Interfaces;
using Multimedia.Web.Dtos;
using X.PagedList;

namespace Multimedia.Web.Controllers
{
    [Authorize]
    [TypeFilter(typeof(GlobalExceptionFilter))]
    public class ImageController : Controller
    {
        private readonly IImagesService _imagesService;
        private readonly IUsersService _usersService;

        public ImageController(IImagesService imagesService, IUsersService usersService)
        {
            _imagesService = imagesService;
            _usersService = usersService;
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var imageDto = await _imagesService.GetImage<ImageDto>(id, User.GetToken());
            byte[] imageBytes = System.IO.File.ReadAllBytes(imageDto.Path);

            return File(imageBytes, "image/jpeg");
        }

        [HttpGet]
        public async Task<IActionResult> Hashtag(int? page, string hashtag)
        {
            ViewData["Hashtag"] = hashtag;
            var pageNumber = page ?? 1;

            var images = await _imagesService.GetImagesByHashtag<List<ImageDto>>(new HashtagDto { Hashtag = hashtag }, User.GetToken());

            var imagesOnPage = images.ToPagedList(pageNumber, 9);

            return View(imagesOnPage);
        }

        [HttpGet]
        public async Task<IActionResult> Email(int? page, string email)
        {
            ViewData["Email"] = email;
            var pageNumber = page ?? 1;

            if (!String.IsNullOrEmpty(email))
            {
                var user = await _usersService.GetUserByEmail<UserDto>(new UserEmailDto { Email = email }, User.GetToken());
                var images = await _imagesService.GetUserImages<List<ImageDto>>(new UserIdDto { UserId = user.Id.ToString() }, User.GetToken());

                var imagesOnPage = images.ToPagedList(pageNumber, 9);

                return View(imagesOnPage);
            }

            var emptyImages = new List<ImageDto>();
            return View(emptyImages.ToPagedList(pageNumber, 9));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var image = await _imagesService.GetImage<ImageDto>(id, User.GetToken());
            var user = await _usersService.GetUserById<UserDto>(new UserIdDto { UserId = image.UserId.ToString() }, User.GetToken());
            ViewBag.UserName = user.FirstName + " " + user.LastName;

            ViewBag.Delete = false;
            if (image.UserId.ToString() == User.GetId())
                ViewBag.Delete = true;

            return View(image);
        }

        [HttpGet]
        public async Task<IActionResult> UserImages(int? page)
        {
            var images = await _imagesService.GetUserImages<List<ImageDto>>(new UserIdDto { UserId = User.GetId() }, User.GetToken());

            var pageNumber = page ?? 1;
            var imagesOnPage = images.ToPagedList(pageNumber, 9);

            return View(imagesOnPage);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommandImageDto imageDto, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View();

            if (imageFile == null || imageFile.Length == 0)
            {
                ViewBag.ImageError = "You do not upload photo.";
                return View();
            }

            if (imageFile.ContentType.ToLower() != "image/jpeg" &&
                imageFile.ContentType.ToLower() != "image/jpg" &&
                imageFile.ContentType.ToLower() != "image/png")
            {
                ViewBag.ImageError = "You do not upload photo.";
                return View();
            }

            imageDto.UserId = Guid.Parse(User.GetId());

            using (var stream = imageFile.OpenReadStream())
            {
                await _imagesService.CreateImage(imageDto, stream, imageFile.FileName, User.GetToken());

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CommandImageDto imageDto)
        {
            if (!ModelState.IsValid)
                return View();

            imageDto.UserId = Guid.Parse(User.GetId());

            await _imagesService.UpdateImage<Task>(id, imageDto, User.GetToken());

            return RedirectToAction("Details", new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _imagesService.DeleteImage<Task>(id, new UserIdDto { UserId = User.GetId() }, User.GetToken());

            return RedirectToAction("Index", "Home");
        }
    }
}
