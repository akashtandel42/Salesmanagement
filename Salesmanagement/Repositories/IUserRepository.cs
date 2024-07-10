using Salesmanagement.Models;

namespace Salesmanagement.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string username);
        Task AddUserAsync(User user);
    }
}
