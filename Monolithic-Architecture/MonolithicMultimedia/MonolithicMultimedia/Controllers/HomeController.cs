using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Services.Interfaces;
using System.Threading.Tasks;
using X.PagedList;

namespace MonolithicMultimedia.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IImagesService _imagesService;
        private readonly IVideosService _videosService;

        public HomeController(IImagesService imagesService, IVideosService videosService)
        {
            _imagesService = imagesService;
            _videosService = videosService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var images = await _imagesService.GetImages();

            var pageNumber = page ?? 1;
            var imagesOnPage = images.ToPagedList(pageNumber, 9);

            return View(imagesOnPage);
        }

        [HttpGet]
        public async Task<IActionResult> Video(int? page)
        {
            var videos = await _videosService.GetVideos();

            var pageNumber = page ?? 1;
            var videosOnPage = videos.ToPagedList(pageNumber, 9);

            return View(videosOnPage);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}
