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
    Task CreateHashtagAsync(Hashtag hashtag); // Add this method
    Task UpdateHashtagAsync(Hashtag hashtag); // Add this method
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

        public async Task<List<Tweet>> GetTweetsByHashtagAsync(string label)
        {
            string hashtagLabel = "#" + label;
            _logger.LogInformation($"Searching for tweets with hashtag: {hashtagLabel}");
            var tweets = await _context.Tweets
                .Include(t => t.Author)
                .Where(t => t.Hashtags.Any(h => EF.Functions.ILike(h.Label, hashtagLabel)) && !t.Deleted)
                .OrderByDescending(t => t.Posted)
                .ToListAsync();

            _logger.LogInformation($"Found {tweets.Count} tweets with hashtag: {hashtagLabel}");
            return tweets;
        }

        public async Task<Hashtag?> GetHashtagByLabelAsync(string label)
        {
            string hashtagLabel = "#" + label;
            _logger.LogInformation($"Searching for hashtag: {hashtagLabel}");
            var hashtag = await _context.Hashtags
                                        .Include(h => h.Tweets)
                                        .FirstOrDefaultAsync(h => EF.Functions.ILike(h.Label, hashtagLabel));

            if (hashtag == null)
            {
                _logger.LogWarning($"Hashtag not found: {hashtagLabel}");
            }
            else
            {
                _logger.LogInformation($"Found hashtag: {hashtag.Label}");
            }

            return hashtag;
        }

        public async Task<bool> CheckHashtagExistsAsync(string label)
        {
            string hashtagLabel = "#" + label;
            _logger.LogInformation($"Checking if hashtag exists: {hashtagLabel}");
            var exists = await _context.Hashtags.AnyAsync(h => EF.Functions.ILike(h.Label, hashtagLabel));
            _logger.LogInformation($"Hashtag exists: {exists}");
            return exists;
        }

        public async Task CreateHashtagAsync(Hashtag hashtag)
        {
            _context.Hashtags.Add(hashtag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHashtagAsync(Hashtag hashtag)
        {
            _context.Hashtags.Update(hashtag);
            await _context.SaveChangesAsync();
        }
    }
}
