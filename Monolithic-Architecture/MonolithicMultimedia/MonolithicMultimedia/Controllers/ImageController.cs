using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MonolithicMultimedia.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
