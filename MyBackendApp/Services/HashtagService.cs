
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBackendApp.DTOs;

namespace MyBackendApp.Services
{

public interface IHashtagService
{
    Task<List<HashtagResponseDto>> GetAllHashtagsAsync();
    Task<List<TweetResponseDto>> GetTweetsByHashtagAsync(string label);
    Task<bool> CheckHashtagExistsAsync(string label);
}
}