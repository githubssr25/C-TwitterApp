using AutoMapper;
using MyBackendApp.DTOs;
using MyBackendApp.Entities;
using MyBackendApp.Repositories;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBackendApp.Services
{
    public class TweetServiceImpl : ITweetService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHashtagRepository _hashtagRepository;
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
            await ProcessHashtags(tweet);

            var createdTweet = await _tweetRepository.CreateTweetAsync(tweet);
            return _mapper.Map<TweetResponseDto>(createdTweet);
        }

        public async Task<TweetResponseDto> DeleteTweetAsync(long id, CredentialsDto credentialsDto)
        {
            var tweet = await _tweetRepository.GetTweetByIdAsync(id);
            if (tweet == null || tweet.Deleted) throw new KeyNotFoundException("Tweet not found");

            var user = await _userRepository.GetUserByUsernameAsync(credentialsDto.Username);
            if (user == null || user.Credentials.Password != credentialsDto.Password)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (tweet.Author.Id != user.Id) throw new UnauthorizedAccessException("You are not the author of this tweet");

            tweet.Deleted = true;
            await _tweetRepository.UpdateTweetAsync(tweet);

            return _mapper.Map<TweetResponseDto>(tweet);
        }

        public async Task LikeTweetAsync(long id, CredentialsDto credentialsDto)
        {
            var tweet = await _tweetRepository.GetTweetByIdAsync(id);
            if (tweet == null || tweet.Deleted) throw new KeyNotFoundException("Tweet not found");

            var user = await _userRepository.GetUserByUsernameAsync(credentialsDto.Username);
            if (user == null || user.Credentials.Password != credentialsDto.Password)
                throw new UnauthorizedAccessException("Invalid credentials");

            tweet.LikedByUsers.Add(user);
            await _tweetRepository.UpdateTweetAsync(tweet);
        }

        public async Task<TweetResponseDto> ReplyToTweetAsync(long id, TweetRequestDto tweetRequestDto)
        {
            var originalTweet = await _tweetRepository.GetTweetByIdAsync(id);
            if (originalTweet == null || originalTweet.Deleted) throw new KeyNotFoundException("Tweet not found");

            var user = await _userRepository.GetUserByUsernameAsync(tweetRequestDto.Credentials.Username);
            if (user == null || user.Credentials.Password != tweetRequestDto.Credentials.Password)
                throw new UnauthorizedAccessException("Invalid credentials");

            var replyTweet = new Tweet
            {
                Author = user,
                Content = tweetRequestDto.Content,
                Deleted = false,
                Posted = DateTime.UtcNow,
                InReplyTo = originalTweet
            };

            // Process mentions and hashtags
            await ProcessHashtags(replyTweet);

            var createdReply = await _tweetRepository.CreateTweetAsync(replyTweet);
            return _mapper.Map<TweetResponseDto>(createdReply);
        }

        public async Task<TweetResponseDto> RepostTweetAsync(long id, CredentialsDto credentialsDto)
        {
            var originalTweet = await _tweetRepository.GetTweetByIdAsync(id);
            if (originalTweet == null || originalTweet.Deleted) throw new KeyNotFoundException("Tweet not found");

            var user = await _userRepository.GetUserByUsernameAsync(credentialsDto.Username);
            if (user == null || user.Credentials.Password != credentialsDto.Password)
                throw new UnauthorizedAccessException("Invalid credentials");

            var repostTweet = new Tweet
            {
                Author = user,
                Deleted = false,
                Posted = DateTime.UtcNow,
                RepostOf = originalTweet
            };

            var createdRepost = await _tweetRepository.CreateTweetAsync(repostTweet);
            return _mapper.Map<TweetResponseDto>(createdRepost);
        }

        private async Task ProcessHashtags(Tweet tweet)
        {
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







// //        // Process mentions and hashtags
//             var hashtags = ExtractHashtags(tweet.Content);
//             foreach (var label in hashtags)
//             {
//                 var hashtag = await _hashtagRepository.GetHashtagByLabelAsync(label);
//                 if (hashtag == null)
//                 {
//                     hashtag = new Hashtag { Label = label, FirstUsed = DateTime.UtcNow, LastUsed = DateTime.UtcNow };
//                     await _hashtagRepository.CreateHashtagAsync(hashtag);
//                 }
//                 else
//                 {
//                     hashtag.LastUsed = DateTime.UtcNow;
//                     await _hashtagRepository.UpdateHashtagAsync(hashtag);
//                 }
//                 tweet.Hashtags.Add(hashtag);
//             }

//             var createdTweet = await _tweetRepository.CreateTweetAsync(tweet);
//             return _mapper.Map<TweetResponseDto>(createdTweet);
//         }