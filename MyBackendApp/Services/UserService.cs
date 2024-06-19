using System.Collections.Generic;
using System.Threading.Tasks;
using MyBackendApp.DTOs;

namespace MyBackendApp.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> GetUserByUsernameAsync(string username);
        Task<UserResponseDto> CreateUserAsync(UserRequestDto userRequestDto);
        Task FollowUserAsync(string username, CredentialsDto credentialsDto);
        Task UnFollowUserAsync(string username, CredentialsDto credentialsDto);
        Task<List<TweetResponseDto>> GetTweetsAsync(string username);
        Task<List<TweetResponseDto>> GetFeedAsync(string username);

        //  Task<List<TweetResponseDto>> GetUserFeedAsync(string username); // Add this method
    }
}
