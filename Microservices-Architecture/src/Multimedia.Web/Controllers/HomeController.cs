using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Multimedia.Web.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Multimedia.Web.Exceptions.Filters;
using Multimedia.Web.Services.Interfaces;
using Multimedia.Web.Helpers;
using Multimedia.Web.Services;

namespace Multimedia.Web.Controllers
{
    [Authorize]
    [TypeFilter(typeof(GlobalExceptionFilter))]
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
            var images = await _imagesService.GetImages<List<ImageDto>>(User.GetToken());

            var pageNumber = page ?? 1;
            var imagesOnPage = images.ToPagedList(pageNumber, 9);

            return View(imagesOnPage);
        }

        [HttpGet]
        public async Task<IActionResult> Video(int? page)
        {
            var videos = await _videosService.GetVideos<List<VideoDto>>(User.GetToken());

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
