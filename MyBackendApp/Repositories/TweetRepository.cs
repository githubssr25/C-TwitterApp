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
        Task<Tweet> CreateTweetAsync(Tweet tweet);
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
                .Where(t => !t.Deleted)
                .OrderByDescending(t => t.Posted)
                .ToListAsync();
        }

        public async Task<Tweet?> GetTweetByIdAsync(long id)
        {
            return await _context.Tweets
                .Include(t => t.Author)
                .FirstOrDefaultAsync(t => t.Id == id && !t.Deleted);
        }

        public async Task<Tweet> CreateTweetAsync(Tweet tweet)
        {
            _context.Tweets.Add(tweet);
            await _context.SaveChangesAsync();
            return tweet;
        }
    }
}
