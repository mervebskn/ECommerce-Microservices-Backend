using AuthService.Entities;

namespace AuthService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_users.SingleOrDefault(u => u.Username == username));
        }

        public Task Create(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }
    }
}
