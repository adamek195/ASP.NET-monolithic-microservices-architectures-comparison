using Multimedia.Users.Entities;
using System.Threading.Tasks;

namespace Multimedia.Users.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUserById(string userId);

        Task<User> GetUserByEmail(string userId);

        Task<User> AddUser(User newUser);
    }
}
