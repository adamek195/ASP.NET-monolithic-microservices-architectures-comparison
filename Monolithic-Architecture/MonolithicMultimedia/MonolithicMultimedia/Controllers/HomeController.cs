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

        public HomeController(IImagesService imagesService)
        {
            _imagesService = imagesService;
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
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}
