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
        List<UserResponseDto> GetAllUsers();
        UserResponseDto GetUserByUsername(string username);
        UserResponseDto UpdateUserProfile(string username, UserRequestDto userRequestDto);
        UserResponseDto DeleteUser(string username, CredentialsDto credentialsDto);
        bool CheckUsernameExists(string username);
        bool CheckUsernameAvailable(string username);
        UserResponseDto CreateUser(UserRequestDto userRequestDto);
        void FollowUser(string username, CredentialsDto credentialsDto);
        void UnFollowUser(string username, CredentialsDto credentialsDto);
        List<TweetResponseDto> GetTweets(string username);
        List<TweetResponseDto> GetFeed(string username);
        List<TweetResponseDto> GetMentions(string username);
        List<UserResponseDto> GetFollowers(string username);
        List<UserResponseDto> GetFollowing(string username);
    }
}