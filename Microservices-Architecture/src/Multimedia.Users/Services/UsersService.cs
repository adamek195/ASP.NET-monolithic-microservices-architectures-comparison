using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Multimedia.Users.Dtos;
using Multimedia.Users.Entities;
using Multimedia.Users.Exceptions;
using Multimedia.Users.Repositories.Interfaces;
using Multimedia.Users.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;

namespace Multimedia.Users.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public UsersService(IUsersRepository usersRepository, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UserDto> GetUserById(UserIdDto userIdDto)
        {
            if (String.IsNullOrEmpty(userIdDto.UserId))
                throw new ArgumentNullException(nameof(userIdDto.UserId));

            var user = await _usersRepository.GetUserById(userIdDto.UserId);

            if (user == null)
                throw new NotFoundException("User does not exist.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByEmail(UserEmailDto emailDto)
        {
            if(String.IsNullOrEmpty(emailDto.Email))
                throw new ArgumentNullException(nameof(emailDto.Email));

            var user = await _usersRepository.GetUserByEmail(emailDto.Email);

            if (user == null)
                throw new NotFoundException("User does not exist.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUser(CreateUserDto newUserDto)
        {
            var user = await _userManager.FindByEmailAsync(newUserDto.Email);

            if (user != null)
                throw new ConflictException("User with the same email already exists!");

            var newUser = _mapper.Map<User>(newUserDto);

            if (newUser == null)
                throw new ConflictException("User creation failed! Please check user details and try again.");

            await _usersRepository.AddUser(newUser);

            return _mapper.Map<UserDto>(newUser);
        }

        public async Task<string> LoginUser(LoginUserDto loginUserDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            if (user == null)
                throw new NotFoundException("Wrong Email! Please check user details and try again.");

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            if (!passwordCorrect)
                throw new NotFoundException("Wrong Password! Please check user details and try again.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtToken:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("JwtToken:TokenLifeTime")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);

            return token;
        }
    }
}
