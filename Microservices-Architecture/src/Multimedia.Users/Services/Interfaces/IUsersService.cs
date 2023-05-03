using Multimedia.Users.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multimedia.Users.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserDto> GetUserById(string userId);

        Task<UserDto> GetUserByEmail(string email);

        Task<UserDto> CreateUser(CreateUserDto newUserDto);

        Task<string> LoginUser(LoginUserDto newUserDto);
    }
}
