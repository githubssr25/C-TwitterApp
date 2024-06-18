using System;
using System.Collections.Generic;
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

        public async Task<UserResponseDto> CreateUserAsync(UserRequestDto userRequestDto) // Ensure method signature matches
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
                    existingUser.Profile = _mapper.Map<MyBackendApp.Entities.Profile>(userRequestDto.Profile);
                    await _userRepository.UpdateUserAsync(existingUser);
                    return _mapper.Map<UserResponseDto>(existingUser);
                }
                else
                {
                    throw new ArgumentException("Username already taken");
                }
            }

            var newUser = _mapper.Map<User>(userRequestDto);
            newUser.Joined = DateTime.UtcNow;
            await _userRepository.CreateUserAsync(newUser);
            return _mapper.Map<UserResponseDto>(newUser);
        }

        public List<UserResponseDto> GetAllUsers()
        {
            var users = _userRepository.FindAllByDeletedFalse();
            return _mapper.Map<List<UserResponseDto>>(users);
        }

        public UserResponseDto GetUserByUsername(string username)
        {
            var user = _userRepository.FindByCredentialsUsername(username);
            if (user == null) throw new KeyNotFoundException("User not found");
            return _mapper.Map<UserResponseDto>(user);
        }

        public UserResponseDto UpdateUserProfile(string username, UserRequestDto userRequestDto)
        {
            var user = _userRepository.FindByCredentialsUsername(username);
            if (user == null) throw new KeyNotFoundException("User not found");
            _mapper.Map(userRequestDto, user);
            _userRepository.Save(user);
            return _mapper.Map<UserResponseDto>(user);
        }

        public UserResponseDto DeleteUser(string username, CredentialsDto credentialsDto)
        {
            var user = _userRepository.FindByCredentialsUsername(username);
            if (user == null) throw new KeyNotFoundException("User not found");
            user.Deleted = true;
            _userRepository.Save(user);
            return _mapper.Map<UserResponseDto>(user);
        }

        public bool CheckUsernameExists(string username)
        {
            return _userRepository.FindByCredentialsUsername(username) != null;
        }

        public bool CheckUsernameAvailable(string username)
        {
            var user = _userRepository.FindByCredentialsUsername(username);
            return user == null || user.Deleted;
        }

        public void FollowUser(string username, CredentialsDto credentialsDto)
        {
            // Implementation...
        }

        public void UnFollowUser(string username, CredentialsDto credentialsDto)
        {
            // Implementation...
        }

        public List<TweetResponseDto> GetTweets(string username)
        {
            // Implementation...
            return null;
        }

        public List<TweetResponseDto> GetFeed(string username)
        {
            // Implementation...
            return null;
        }

        public List<TweetResponseDto> GetMentions(string username)
        {
            // Implementation...
            return null;
        }

        public List<UserResponseDto> GetFollowers(string username)
        {
            // Implementation...
            return null;
        }

        public List<UserResponseDto> GetFollowing(string username)
        {
            // Implementation...
            return null;
        }
    }
}

