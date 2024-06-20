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

        Task<Tweet?> GetTweetWithLikesByIdAsync(long id); // Add the new method 
        Task<Tweet> CreateTweetAsync(Tweet tweet);

         Task UpdateTweetAsync(Tweet tweet);
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
    }
}