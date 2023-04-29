using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Helpers;
using MonolithicMultimedia.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Controllers
{
    [Authorize]
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
