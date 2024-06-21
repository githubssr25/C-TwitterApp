using MyBackendApp.Data;
using Microsoft.EntityFrameworkCore;
using MyBackendApp.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MyBackendApp.Repositories
{
    public interface IHashtagRepository
    {
        Task<List<Hashtag>> GetAllHashtagsAsync();
        Task<Hashtag?> GetHashtagByLabelAsync(string label);
        Task<List<Tweet>> GetTweetsByHashtagAsync(string label);
        Task<bool> CheckHashtagExistsAsync(string label);
        Task CreateHashtagAsync(Hashtag hashtag);
        Task UpdateHashtagAsync(Hashtag hashtag);
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
    string hashtagLabel = "#" + label.Trim('#').Trim(); // Normalize and trim the label
    _logger.LogInformation($"Searching for tweets with hashtag: {hashtagLabel}");

    // Log each tweet and its content for debugging
    var allTweets = await _context.Tweets
        .Include(t => t.Author)
        .Include(t => t.Hashtags)
        .Where(t => !t.Deleted)
        .ToListAsync();

    foreach (var tweet in allTweets)
    {
        _logger.LogInformation($"Tweet Content: {tweet.Content}");

        var hashtags = ExtractHashtags(tweet.Content);
        foreach (var hashtag in hashtags)
        {
            _logger.LogInformation($"Extracted Hashtag: {hashtag}");
        }
    }

    var tweets = allTweets
        .Where(t => t.Hashtags.Any(h => EF.Functions.ILike(h.Label, hashtagLabel)))
        .OrderByDescending(t => t.Posted)
        .ToList();

    _logger.LogInformation($"Found {tweets.Count} tweets with hashtag: {hashtagLabel}");
    return tweets;
}

     public async Task<Hashtag?> GetHashtagByLabelAsync(string label)
{
    string hashtagLabel = "#" + label.Trim('#').Trim(); // Normalize and trim the label
    _logger.LogInformation($"Searching for hashtag: {hashtagLabel}");

    // Log each hashtag for debugging
    var allHashtags = await _context.Hashtags.Include(h => h.Tweets).ToListAsync();
    foreach (var hashtag in allHashtags)
    {
        _logger.LogInformation($"Hashtag: {hashtag.Label}");
    }

    var hashtagResult = await _context.Hashtags
        .Include(h => h.Tweets)
        .FirstOrDefaultAsync(h => EF.Functions.ILike(h.Label, hashtagLabel));

    if (hashtagResult == null)
    {
        _logger.LogWarning($"Hashtag not found: {hashtagLabel}");
    }
    else
    {
        _logger.LogInformation($"Found hashtag: {hashtagResult.Label}");
    }

    return hashtagResult;
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

     private List<string> ExtractHashtags(string content)
{
    var hashtags = new List<string>();
    var matches = System.Text.RegularExpressions.Regex.Matches(content, @"#\w+");
    foreach (System.Text.RegularExpressions.Match match in matches)
    {
        hashtags.Add(match.Value.Trim('#').Trim());
    }
    return hashtags;
}

    }
}
