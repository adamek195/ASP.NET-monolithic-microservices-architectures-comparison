using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Multimedia.Web.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Multimedia.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var images = new List<ImageDto>();
            return View(images.ToPagedList(1, 9));
        }
    }
}
