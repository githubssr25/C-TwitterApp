using AutoMapper;
using MyBackendApp.DTOs;
using MyBackendApp.Entities;
using MyBackendApp.Repositories;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; // Add this for Regex and Match
using System.Threading.Tasks;

namespace MyBackendApp.Services
{
    public class TweetServiceImpl : ITweetService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHashtagRepository _hashtagRepository; // Add this to interact with hashtags
        private readonly IMapper _mapper;

        public TweetServiceImpl(ITweetRepository tweetRepository, IUserRepository userRepository, IHashtagRepository hashtagRepository, IMapper mapper)
        {
            _tweetRepository = tweetRepository;
            _userRepository = userRepository;
            _hashtagRepository = hashtagRepository;
            _mapper = mapper;
        }

        public async Task<List<TweetResponseDto>> GetAllTweetsAsync()
        {
            var tweets = await _tweetRepository.GetAllTweetsAsync();
            return _mapper.Map<List<TweetResponseDto>>(tweets);
        }

        public async Task<TweetResponseDto?> GetTweetByIdAsync(long id)
        {
            var tweet = await _tweetRepository.GetTweetByIdAsync(id);
            if (tweet == null) throw new KeyNotFoundException("Tweet not found");
            return _mapper.Map<TweetResponseDto>(tweet);
        }

        public async Task<TweetResponseDto> CreateTweetAsync(TweetRequestDto tweetRequestDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(tweetRequestDto.Credentials.Username);
            if (user == null || user.Credentials.Password != tweetRequestDto.Credentials.Password)
                throw new UnauthorizedAccessException("Invalid credentials");

            var tweet = new Tweet
            {
                Author = user,
                Content = tweetRequestDto.Content,
                Deleted = false,
                Posted = DateTime.UtcNow
            };

            // Process mentions and hashtags
            var hashtags = ExtractHashtags(tweet.Content);
            foreach (var label in hashtags)
            {
                var hashtag = await _hashtagRepository.GetHashtagByLabelAsync(label);
                if (hashtag == null)
                {
                    hashtag = new Hashtag { Label = label, FirstUsed = DateTime.UtcNow, LastUsed = DateTime.UtcNow };
                    await _hashtagRepository.CreateHashtagAsync(hashtag);
                }
                else
                {
                    hashtag.LastUsed = DateTime.UtcNow;
                    await _hashtagRepository.UpdateHashtagAsync(hashtag);
                }
                tweet.Hashtags.Add(hashtag);
            }

            var createdTweet = await _tweetRepository.CreateTweetAsync(tweet);
            return _mapper.Map<TweetResponseDto>(createdTweet);
        }

        private List<string> ExtractHashtags(string content)
        {
            var hashtags = new List<string>();
            var matches = Regex.Matches(content, @"#\w+");
            foreach (Match match in matches)
            {
                hashtags.Add(match.Value);
            }
            return hashtags;
        }
    }
}
