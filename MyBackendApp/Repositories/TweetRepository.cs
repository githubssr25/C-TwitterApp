using MyBackendApp.Data;
using Microsoft.EntityFrameworkCore;
using MyBackendApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBackendApp.Repositories
{
    public interface ITweetRepository
    {
   Task<List<Tweet>> GetAllTweetsAsync();
    Task<Tweet?> GetTweetByIdAsync(long id);
    Task<List<Tweet>> GetRepliesByTweetIdAsync(long id);
    Task<Tweet?> GetTweetWithLikesByIdAsync(long id);
    Task<Tweet> CreateTweetAsync(Tweet tweet);
    Task UpdateTweetAsync(Tweet tweet);
    Task<List<Tweet>> GetRepostsByTweetIdAsync(long id);

     Task<List<User>> GetMentionsByTweetIdAsync(long id); // Add this method

    }

     public class TweetRepository : ITweetRepository
    {
        private readonly AppDbContext _context;

        public TweetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tweet>> GetAllTweetsAsync()
        {
            return await _context.Tweets
                .Include(t => t.Author)
                .Include(t => t.Hashtags)
                .Where(t => !t.Deleted)
                .OrderByDescending(t => t.Posted)
                .ToListAsync();
        }

        public async Task<Tweet?> GetTweetByIdAsync(long id)
        {
            return await _context.Tweets
                .Include(t => t.Author)
                .Include(t => t.Hashtags)
                .FirstOrDefaultAsync(t => t.Id == id && !t.Deleted);
        }

        public async Task<Tweet?> GetTweetWithLikesByIdAsync(long id)
{
    return await _context.Tweets
        .Include(t => t.Author)
        .Include(t => t.Hashtags)
        .Include(t => t.LikedByUsers) // Include liked users
        .FirstOrDefaultAsync(t => t.Id == id && !t.Deleted);
}

        public async Task<Tweet> CreateTweetAsync(Tweet tweet)
        {
            _context.Tweets.Add(tweet);
            await _context.SaveChangesAsync();
            return tweet;
        }

        public async Task UpdateTweetAsync(Tweet tweet)
        {
            _context.Tweets.Update(tweet);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Tweet>> GetRepliesByTweetIdAsync(long id)
        {
            return await _context.Tweets
                .Include(t => t.Author)
                .Include(t => t.Hashtags)
                .Where(t => t.InReplyTo.Id == id && !t.Deleted)
                .ToListAsync();
        }

        public async Task<List<Tweet>> GetRepostsByTweetIdAsync(long id)
        {
            return await _context.Tweets
                .Include(t => t.Author)
                .Include(t => t.Hashtags)
                .Where(t => t.RepostOf.Id == id && !t.Deleted)
                .ToListAsync();
        }

   public async Task<List<User>> GetMentionsByTweetIdAsync(long id)
{
    var mentionedUsers = await _context.Users
        .FromSqlRaw(@"
            SELECT u.*
            FROM ""user_account"" u
            INNER JOIN ""user_mentions"" um ON u.""Id"" = um.""MentionedUsersId""
            WHERE um.""MentionedTweetsId"" = {0} AND u.""Deleted"" = false", id)
        .ToListAsync();

    return mentionedUsers;
}

    }
}