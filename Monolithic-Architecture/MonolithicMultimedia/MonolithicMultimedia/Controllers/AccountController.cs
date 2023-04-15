using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Exceptions.Filters;
using MonolithicMultimedia.Services.Interfaces;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Controllers
{
    [TypeFilter(typeof(GlobalExceptionFilter))]
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateUserDto newUserDto)
        {
            var newUser = await _usersService.CreateUser(newUserDto);

            ViewBag.UserName = newUser.FirstName + " " + newUser.LastName;
            ViewBag.Email = newUser.Email;
            ViewBag.UserRegister = "Registration was successful!";
            
            return View("Registered");
        }
    }
}
