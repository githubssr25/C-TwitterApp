using MyBackendApp.Data;
using Microsoft.EntityFrameworkCore;
using MyBackendApp.Entities;
using Microsoft.Extensions.Logging;

namespace MyBackendApp.Repositories
{
    public interface IHashtagRepository
    {
        Task<List<Hashtag>> GetAllHashtagsAsync();
        Task<Hashtag?> GetHashtagByLabelAsync(string label);
        Task<List<Tweet>> GetTweetsByHashtagAsync(string label);
        Task<bool> CheckHashtagExistsAsync(string label);
    }

    public class HashtagRepository : IHashtagRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HashtagRepository> _logger;

        public HashtagRepository(AppDbContext context, ILogger<HashtagRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Hashtag>> GetAllHashtagsAsync()
        {
            return await _context.Hashtags.ToListAsync();
        }

        public async Task<Hashtag?> GetHashtagByLabelAsync(string label)
        {
            _logger.LogInformation($"Searching for hashtag: {label}");
            var hashtag = await _context.Hashtags
                                        .Include(h => h.Tweets)
                                        .FirstOrDefaultAsync(h => EF.Functions.ILike(h.Label, label));

            if (hashtag == null)
            {
                _logger.LogWarning($"Hashtag not found: {label}");
            }
            else
            {
                _logger.LogInformation($"Found hashtag: {hashtag.Label}");
            }

            return hashtag;
        }

        public async Task<List<Tweet>> GetTweetsByHashtagAsync(string label)
        {
            _logger.LogInformation($"Searching for tweets with hashtag: {label}");
            var tweets = await _context.Tweets
                .Include(t => t.Author)
                .Where(t => t.Hashtags.Any(h => EF.Functions.ILike(h.Label, label)) && !t.Deleted)
                .OrderByDescending(t => t.Posted)
                .ToListAsync();

            _logger.LogInformation($"Found {tweets.Count} tweets with hashtag: {label}");
            return tweets;
        }

        public async Task<bool> CheckHashtagExistsAsync(string label)
        {
            _logger.LogInformation($"Checking if hashtag exists: {label}");
            var exists = await _context.Hashtags.AnyAsync(h => EF.Functions.ILike(h.Label, label));
            _logger.LogInformation($"Hashtag exists: {exists}");
            return exists;
        }
    }
}
