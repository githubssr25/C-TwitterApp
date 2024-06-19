using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyBackendApp.DTOs;
using MyBackendApp.Entities;
using MyBackendApp.Repositories;

namespace MyBackendApp.Services
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserServiceImpl(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) throw new KeyNotFoundException("User not found");
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(UserRequestDto userRequestDto)
        {
            if (userRequestDto == null || userRequestDto.Credentials == null || userRequestDto.Profile == null)
            {
                throw new ArgumentException("Missing user data, credentials, or profile information.");
            }

            var credentials = userRequestDto.Credentials;
            var existingUser = _userRepository.FindByCredentialsUsername(credentials.Username);

            if (existingUser != null)
            {
                if (existingUser.Deleted)
                {
                    // Reactivate deleted user
                    existingUser.Deleted = false;
                    existingUser.Profile = _mapper.Map<MyBackendApp.Entities.Profile>(userRequestDto.Profile); // Fully qualify
                    await _userRepository.UpdateUserAsync(existingUser);
                    return _mapper.Map<UserResponseDto>(existingUser);
                }
                else
                {
                    throw new ArgumentException("Username already taken");
                }
            }

            var newUser = _mapper.Map<MyBackendApp.Entities.User>(userRequestDto);
            newUser.Joined = DateTime.UtcNow;
            await _userRepository.CreateUserAsync(newUser);
            return _mapper.Map<UserResponseDto>(newUser);
        }



        public async Task FollowUserAsync(string username, CredentialsDto credentialsDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(credentialsDto.Username);
            if (user == null || user.Deleted)
                throw new ArgumentException("Invalid credentials");

            var userToFollow = await _userRepository.GetUserByUsernameAsync(username);
            if (userToFollow == null || userToFollow.Deleted)
                throw new KeyNotFoundException("User to follow not found");

            if (user.Following.Contains(userToFollow))
                throw new InvalidOperationException("Already following this user");

            user.Following.Add(userToFollow);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task UnFollowUserAsync(string username, CredentialsDto credentialsDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(credentialsDto.Username);
            if (user == null || user.Deleted)
                throw new ArgumentException("Invalid credentials");

            var userToUnfollow = await _userRepository.GetUserByUsernameAsync(username);
            if (userToUnfollow == null || userToUnfollow.Deleted)
                throw new KeyNotFoundException("User to unfollow not found");

            if (!user.Following.Contains(userToUnfollow))
                throw new InvalidOperationException("Not following this user");

            user.Following.Remove(userToUnfollow);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<List<TweetResponseDto>> GetTweetsAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.Deleted)
                throw new KeyNotFoundException("User not found");

            var tweets = user.Tweets.Where(t => !t.Deleted).OrderByDescending(t => t.Posted).ToList();
            return _mapper.Map<List<TweetResponseDto>>(tweets);
        }

        public async Task<List<TweetResponseDto>> GetFeedAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || user.Deleted)
                throw new KeyNotFoundException("User not found");

            var tweets = user.Tweets
                .Where(t => !t.Deleted)
                .Concat(user.Following.SelectMany(f => f.Tweets).Where(t => !t.Deleted))
                .OrderByDescending(t => t.Posted)
                .ToList();

            return _mapper.Map<List<TweetResponseDto>>(tweets);
        }
    }
}
