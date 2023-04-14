using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Services.Interfaces;

namespace MonolithicMultimedia.Controllers
{
    public class AccountController : Controller
    {
        private IUsersService _usersService;

        public AccountController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
