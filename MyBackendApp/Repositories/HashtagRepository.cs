using MyBackendApp.Data;
using Microsoft.EntityFrameworkCore;
using MyBackendApp.Entities;

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

        public HashtagRepository(AppDbContext context)
        {
            _context = context;
        }

           public async Task<List<Tweet>> GetTweetsByHashtagAsync(string label)
        {
            return await _context.Tweets
                .Include(t => t.Author)
                .Where(t => t.Hashtags.Any(h => h.Label == label) && !t.Deleted)
                .OrderByDescending(t => t.Posted)
                .ToListAsync();
        }

        public async Task<List<Hashtag>> GetAllHashtagsAsync()
        {
            return await _context.Hashtags.ToListAsync();
        }

        public async Task<Hashtag?> GetHashtagByLabelAsync(string label)
        {
            return await _context.Hashtags
                                 .Include(h => h.Tweets)
                                 .FirstOrDefaultAsync(h => h.Label == label);
        }

        public async Task<bool> CheckHashtagExistsAsync(string label)
        {
            return await _context.Hashtags.AnyAsync(h => h.Label == label);
        }
    }
}
