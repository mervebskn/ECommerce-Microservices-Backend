using AuthService.Entities;

namespace AuthService.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(string username);
        Task Create(User user);
    }
}
