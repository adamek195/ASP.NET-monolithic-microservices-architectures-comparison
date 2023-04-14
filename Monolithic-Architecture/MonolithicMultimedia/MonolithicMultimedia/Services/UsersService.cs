using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Entities;
using MonolithicMultimedia.Exceptions;
using MonolithicMultimedia.Repositories.Interfaces;
using MonolithicMultimedia.Services.Interfaces;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UsersService(IUsersRepository usersRepository, IMapper mapper, UserManager<User> userManager)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserDto> GetUser(string userId)
        {
            var user = await _usersRepository.GetUser(userId);

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
    }
}
