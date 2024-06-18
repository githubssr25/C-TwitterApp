using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBackendApp.Data;
using MyBackendApp.Entities;

namespace MyBackendApp.Repositories
{
    public interface IUserRepository
    {
        User? FindByCredentialsUsername(string username);
        List<User> FindAllByDeletedFalse();
        List<User> FindAllByDeletedTrue();
        void Save(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);

        Task UpdateUserAsync(User user); // Add this method
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Where(u => !u.Deleted).ToListAsync();
        }

        public void Save(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Credentials)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Credentials.Username == username && !u.Deleted);
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public User? FindByCredentialsUsername(string username)
        {
            return _context.Users.Include(u => u.Credentials).FirstOrDefault(u => u.Credentials.Username == username);
        }

        public List<User> FindAllByDeletedFalse()
        {
            return _context.Users.Where(u => !u.Deleted).ToList();
        }

        public List<User> FindAllByDeletedTrue()
        {
            return _context.Users.Where(u => u.Deleted).ToList();
        }

         public async Task UpdateUserAsync(User user) // Add this method
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
