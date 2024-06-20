using MyBackendApp.DTOs;
using MyBackendApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBackendApp.Services
{
    public interface ITweetService
    {
        Task<List<TweetResponseDto>> GetAllTweetsAsync();
        Task<TweetResponseDto?> GetTweetByIdAsync(long id);
        Task<TweetResponseDto> CreateTweetAsync(TweetRequestDto tweetRequestDto);
        Task<TweetResponseDto> DeleteTweetAsync(long id, CredentialsDto credentialsDto);
        Task LikeTweetAsync(long id, CredentialsDto credentialsDto);
        Task<TweetResponseDto> ReplyToTweetAsync(long id, TweetRequestDto tweetRequestDto);
        Task<TweetResponseDto> RepostTweetAsync(long id, CredentialsDto credentialsDto);
    }
}