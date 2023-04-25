using MonolithicMultimedia.Entities;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUserById(string userId);

        Task<User> GetUserByEmail(string userId);

        Task<User> AddUser(User newUser);
    }
}
