using MonolithicMultimedia.Dtos;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserDto> GetUserById(string userId);

        Task<UserDto> GetUserByEmail(string email);

        Task<UserDto> CreateUser(CreateUserDto newUserDto);

        Task<bool> LoginUser(LoginUserDto newUserDto);
    }
}
