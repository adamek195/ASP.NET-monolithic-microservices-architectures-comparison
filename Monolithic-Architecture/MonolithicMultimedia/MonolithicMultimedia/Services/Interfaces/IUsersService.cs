using MonolithicMultimedia.Dtos;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserDto> GetUser(string userId);

        Task<UserDto> CreateUser(CreateUserDto newUserDto);
    }
}
