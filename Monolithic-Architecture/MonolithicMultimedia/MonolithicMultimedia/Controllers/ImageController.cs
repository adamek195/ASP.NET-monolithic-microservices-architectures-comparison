using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Dtos;

namespace MonolithicMultimedia.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CommandImageDto imageDto, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View();

            return View();
        }
    }
}
