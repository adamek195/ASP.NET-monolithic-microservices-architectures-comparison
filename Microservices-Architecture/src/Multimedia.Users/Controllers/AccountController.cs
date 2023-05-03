using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Multimedia.Users.Dtos;
using Multimedia.Users.Exceptions.Filters;
using Multimedia.Users.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            var token = await _usersService.LoginUser(loginUserDto);

            if (token == null)
                return BadRequest();

            return Ok(token);
        }
    }
}
