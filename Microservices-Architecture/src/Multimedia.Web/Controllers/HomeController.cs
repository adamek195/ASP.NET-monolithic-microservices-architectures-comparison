using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Multimedia.Web.Dtos;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace Multimedia.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            var images = new List<ImageDto>();
            return View(images.ToPagedList(1, 9));
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}
