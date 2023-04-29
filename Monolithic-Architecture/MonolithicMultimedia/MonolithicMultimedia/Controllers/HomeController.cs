using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Services.Interfaces;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IImagesService _imagesService;

        public HomeController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var images = await _imagesService.GetImages();

            return View(images);
        }

        [HttpGet]
        public async Task<IActionResult> Hashtag(string hashtag)
        {
            var images = await _imagesService.GetImagesByHashtag(hashtag);

            return View(images);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}
