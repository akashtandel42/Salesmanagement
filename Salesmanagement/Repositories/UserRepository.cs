using Salesmanagement.Models;

namespace Salesmanagement.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<User> GetUserAsync(string username)
        {
            return Task.FromResult(_users.SingleOrDefault(u => u.Username == username));
        }

        public Task AddUserAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }
    }
}
