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
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByUsernameAsync(string username);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    void Save(User user);
    Task<List<Tweet>> GetMentionsByUserIdAsync(int userId);
    Task<List<User>> GetFollowersAsync(int userId);

    Task<List<User>> GetFollowingAsync(int userId);

         Task<List<Tweet>> GetFeedByUserIdAsync(int userId);

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

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Credentials)
            .Include(u => u.Profile)
            .Include(u => u.Following)
            .Include(u => u.Followers)
            .Include(u => u.Tweets)
            .FirstOrDefaultAsync(u => u.Credentials.Username == username && !u.Deleted);
    }

    public async Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public User? FindByCredentialsUsername(string username)
    {
        return _context.Users.Include(u => u.Credentials).FirstOrDefault(u => u.Credentials.Username == username);
    }

    public void Save(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

        public async Task<List<Tweet>> GetMentionsByUserIdAsync(int userId)
        {
            var tweets = await _context.Tweets
                .FromSqlRaw(@"
            SELECT t.*
            FROM tweet t
            INNER JOIN user_mentions um ON t.""Id"" = um.""MentionedTweetsId""
            WHERE um.""MentionedUsersId"" = {0} AND t.""Deleted"" = FALSE
            ORDER BY t.""Posted"" DESC", userId)
                .ToListAsync();

            return tweets;
        }


        public async Task<List<User>> GetFollowersAsync(int userId)
    {
        return await _context.Users
            .Where(u => u.Following.Any(f => f.Id == userId) && !u.Deleted)
            .ToListAsync();
    }

    public async Task<List<User>> GetFollowingAsync(int userId)
{
    return await _context.Users
        .Where(u => u.Followers.Any(f => f.Id == userId) && !u.Deleted)
        .ToListAsync();
}

    public async Task<List<Tweet>> GetFeedByUserIdAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Tweets)
                .Include(u => u.Following)
                .ThenInclude(f => f.Tweets)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.Deleted);

            if (user == null)
            {
                return new List<Tweet>();
            }

            var feed = user.Tweets
                .Concat(user.Following.SelectMany(f => f.Tweets))
                .Where(t => !t.Deleted)
                .OrderByDescending(t => t.Posted)
                .ToList();

            return feed;
        }

}
}