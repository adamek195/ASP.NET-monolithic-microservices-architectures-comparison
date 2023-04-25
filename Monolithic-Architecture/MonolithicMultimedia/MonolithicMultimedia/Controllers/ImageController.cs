using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Exceptions;
using MonolithicMultimedia.Helpers;
using MonolithicMultimedia.Services.Interfaces;
using System;
using System.IO;

namespace MonolithicMultimedia.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly IImagesService _imagesService;

        public ImageController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CommandImageDto imageDto, IFormFile imageFile)
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
                _imagesService.CreateImage(imageDto, stream, User.GetId(), imageFile.FileName);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
