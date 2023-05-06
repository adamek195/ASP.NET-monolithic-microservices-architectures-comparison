using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Multimedia.Web.Services.Interfaces;
using Multimedia.Web.Dtos;
using Multimedia.Web.Exceptions.Filters;

namespace Multimedia.Web.Controllers
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
            if (!ModelState.IsValid)
                return View();

            var user = await _usersService.CreateUser<UserDto>(newUserDto);


            ViewBag.UserName = user.FirstName + " " + user.LastName;
            ViewBag.Email = user.Email;
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
            if (!ModelState.IsValid)
                return View();

            var token = await _usersService.LoginUser<TokenDto>(loginUserDto);

            if (token != null)
            {
                var user = await _usersService.GetUserByEmail<UserDto>(new UserEmailDto { Email = loginUserDto.Email }, token.Token);

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("Token", token.Token)
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

            ViewData["UserNotFoundMessage"] = "User not found!";

            return View();
        }
    }
}
