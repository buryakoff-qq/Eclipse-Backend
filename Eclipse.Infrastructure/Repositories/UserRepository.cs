using Eclipse.Core.Interfaces.IUser;
using Eclipse.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Eclipse.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EclipseDbContext _context;

        public UserRepository(EclipseDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
