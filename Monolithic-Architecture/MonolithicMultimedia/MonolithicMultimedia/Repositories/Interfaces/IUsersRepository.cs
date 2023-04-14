using MonolithicMultimedia.Entities;
using System.Threading.Tasks;

namespace MonolithicMultimedia.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUser(string userId);

        Task<User> AddUser(User newUser);
    }
}
