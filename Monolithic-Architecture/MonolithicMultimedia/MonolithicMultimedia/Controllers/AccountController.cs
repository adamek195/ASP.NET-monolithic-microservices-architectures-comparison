using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Exceptions.Filters;
using MonolithicMultimedia.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
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
        [AllowAnonymous]
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (await _usersService.LoginUser(loginUserDto))
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, loginUserDto.Email)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }

            ViewData["ErrorMessage"] = "User not found!";

            return View();
        }
    }
}
