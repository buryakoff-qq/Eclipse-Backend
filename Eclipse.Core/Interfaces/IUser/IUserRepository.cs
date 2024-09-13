using Eclipse.Core.Models;

namespace Eclipse.Core.Interfaces.IUser
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(int userId);
        public Task<User> GetUserByUsername(string username);
        public Task<User> GetUserByEmail(string email);
        public Task CreateUser(User user);
        public Task UpdateUser(User user);
        public Task DeleteUser(int userId);
    }
}
