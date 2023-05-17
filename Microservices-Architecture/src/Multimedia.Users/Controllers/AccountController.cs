using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Multimedia.Users.Dtos;
using Multimedia.Users.Exceptions.Filters;
using Multimedia.Users.Services.Interfaces;
using System.Threading.Tasks;

namespace Multimedia.Users.Controllers
{
    [ApiController]
    [GlobalExceptionFilter]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AccountController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserDto newUserDto)
        {
            var newUser = await _usersService.CreateUser(newUserDto);

            return Created($"api/users/{newUser.Id}", newUser);
        }

        [HttpPost]
        [Route("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var token = await _usersService.LoginUser(loginUserDto);

            if (token == null)
                return BadRequest();

            return Ok(new TokenDto { Token = token});
        }

        [HttpGet]
        [Route("Email")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Email([FromBody] UserEmailDto emailDto)
        {
            var user = await _usersService.GetUserByEmail(emailDto);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }

        [HttpGet]
        [Route("User")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> User([FromBody] UserIdDto userDto)
        {
            var user = await _usersService.GetUserById(userDto);

            if (user == null)
                return BadRequest();

            return Ok(user);
        }
    }
}
