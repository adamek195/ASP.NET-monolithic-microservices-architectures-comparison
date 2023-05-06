using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Exceptions.Filters;
using MonolithicMultimedia.Helpers;
using MonolithicMultimedia.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace MonolithicMultimedia.Controllers
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
            var imageDto = await _imagesService.GetImage(id);
            byte[] imageBytes = System.IO.File.ReadAllBytes(imageDto.Path);

            return File(imageBytes, "image/jpeg");
        }

        [HttpGet]
        public async Task<IActionResult> Hashtag(int? page, string hashtag)
        {
            ViewData["Hashtag"] = hashtag;
            var pageNumber = page ?? 1;

            var images = await _imagesService.GetImagesByHashtag(hashtag);

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
                var images = await _imagesService.GetImagesByEmail(email);

                var imagesOnPage = images.ToPagedList(pageNumber, 9);

                return View(imagesOnPage);
            }

            var emptyImages = new List<ImageDto>();
            return View(emptyImages.ToPagedList(pageNumber, 9));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var imageDto = await _imagesService.GetImage(id);
            var userDto = await _usersService.GetUserById(imageDto.UserId.ToString());
            ViewBag.UserName = userDto.FirstName + " " + userDto.LastName;

            ViewBag.Delete = false;
            if (imageDto.UserId.ToString() == User.GetId())
                ViewBag.Delete = true;

            return View(imageDto);
        }

        [HttpGet]
        public async Task<IActionResult> UserImages(int? page)
        {
            var images = await _imagesService.GetUserImages(User.GetId());

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

            using (var stream = imageFile.OpenReadStream())
            {
                await _imagesService.CreateImage(imageDto, stream, User.GetId(), imageFile.FileName);

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CommandImageDto imageDto)
        {
            if (!ModelState.IsValid)
                return View();

            await _imagesService.UpdateImage(id, User.GetId(), imageDto);

            return RedirectToAction("Details", new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _imagesService.DeleteImage(id, User.GetId());

            return RedirectToAction("Index", "Home");
        }
    }
}
