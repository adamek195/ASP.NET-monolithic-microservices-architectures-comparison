using Multimedia.Web.Dtos;
using System.Threading.Tasks;

namespace Multimedia.Web.Services.Interfaces
{
    public interface IUsersService
    {
        Task<T> GetUserByEmail<T>(UserEmailDto emailDto, string token = null);

        Task<T> CreateUser<T>(CreateUserDto newUserDto, string token = null);

        Task<T> LoginUser<T>(LoginUserDto loginUserDto, string token = null);
    }
}
