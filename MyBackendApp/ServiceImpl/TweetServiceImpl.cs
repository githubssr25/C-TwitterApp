using AutoMapper;
using MyBackendApp.DTOs;
using MyBackendApp.Entities;
using MyBackendApp.Repositories;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TweetServiceImpl> _logger;

        public TweetServiceImpl(ITweetRepository tweetRepository, IUserRepository userRepository, IHashtagRepository hashtagRepository, IMapper mapper, ILogger<TweetServiceImpl> logger)
        {
            _tweetRepository = tweetRepository;
            _userRepository = userRepository;
            _hashtagRepository = hashtagRepository;
            _mapper = mapper;
            _logger = logger;
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
            ProcessHashtags(tweet);

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
               _logger.LogInformation("Like tweet LOGGING 153 6-20 received with ID: {Id} and credentials: Username: {Username}, Password: {Password}", id, credentialsDto?.Username, credentialsDto?.Password);
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


    _logger.LogInformation("Repost request received with ID: {Id} and credentials: Username: {Username}, Password: {Password}", id, credentialsDto?.Username, credentialsDto?.Password);

    if (credentialsDto == null)
    {
        _logger.LogWarning("CredentialsDto is null.");
        throw new UnauthorizedAccessException("Invalid credentials");
    }

    if (string.IsNullOrEmpty(credentialsDto.Username))
    {
        _logger.LogWarning("Username is missing.");
        throw new UnauthorizedAccessException("Invalid credentials");
    }

    if (string.IsNullOrEmpty(credentialsDto.Password))
    {
        _logger.LogWarning("Password is missing.");
        throw new UnauthorizedAccessException("Invalid credentials");
    }

    var originalTweet = await _tweetRepository.GetTweetByIdAsync(id);
    if (originalTweet == null || originalTweet.Deleted)
    {
        _logger.LogWarning("Tweet not found or deleted for ID: {Id}", id);
        throw new KeyNotFoundException("Tweet not found");
    }

    var user = await _userRepository.GetUserByUsernameAsync(credentialsDto.Username);
    if (user == null || user.Credentials.Password != credentialsDto.Password)
    {
        _logger.LogWarning("Invalid credentials for user: {Username}", credentialsDto.Username);
        throw new UnauthorizedAccessException("Invalid credentials");
    }

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


        public async Task<List<HashtagResponseDto>> GetTagsByTweetIdAsync(long id)
        {
            var tweet = await _tweetRepository.GetTweetByIdAsync(id);
            if (tweet == null || tweet.Deleted) throw new KeyNotFoundException("Tweet not found");

            var hashtags = _mapper.Map<List<HashtagResponseDto>>(tweet.Hashtags);
            return hashtags;
        }

      public async Task<List<UserResponseDto>> GetLikesByTweetIdAsync(long id)
{
    var tweet = await _tweetRepository.GetTweetWithLikesByIdAsync(id); // Use the new method
    if (tweet == null || tweet.Deleted)
    {
        _logger.LogWarning("Tweet not found or is deleted: {TweetId}", id);
        throw new KeyNotFoundException("Tweet not found");
    }

    _logger.LogInformation("Fetched tweet: {TweetId} with likes count: {LikesCount}", tweet.Id, tweet.LikedByUsers.Count);

    var activeUsers = tweet.LikedByUsers
        .Where(user => !user.Deleted)
        .ToList();

    _logger.LogInformation("Active users who liked the tweet: {UserCount}", activeUsers.Count);

    return _mapper.Map<List<UserResponseDto>>(activeUsers);
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

        public async Task<List<TweetResponseDto>> GetTweetContextByIdAsync(long id)
        {
            var tweet = await _tweetRepository.GetTweetByIdAsync(id);
            if (tweet == null || tweet.Deleted) throw new KeyNotFoundException("Tweet not found");

            var contextTweets = new List<Tweet>();
            var currentTweet = tweet;

            // Get the before context
            while (currentTweet.InReplyTo != null && !currentTweet.InReplyTo.Deleted)
            {
                contextTweets.Add(currentTweet.InReplyTo);
                currentTweet = currentTweet.InReplyTo;
            }

            // Get the after context
            await GetAfterContext(tweet, contextTweets);

            return _mapper.Map<List<TweetResponseDto>>(contextTweets);
        }

        private async Task GetAfterContext(Tweet tweet, List<Tweet> contextTweets)
        {
            var replies = await _tweetRepository.GetRepliesByTweetIdAsync(tweet.Id);
            foreach (var reply in replies)
            {
                if (!reply.Deleted)
                {
                    contextTweets.Add(reply);
                    await GetAfterContext(reply, contextTweets);
                }
            }
        }

        public async Task<List<TweetResponseDto>> GetRepliesByTweetIdAsync(long id)
        {
            var tweet = await _tweetRepository.GetTweetByIdAsync(id);
            if (tweet == null || tweet.Deleted) throw new KeyNotFoundException("Tweet not found");

            var replies = await _tweetRepository.GetRepliesByTweetIdAsync(id);
            var activeReplies = replies.Where(reply => !reply.Deleted).ToList();

            return _mapper.Map<List<TweetResponseDto>>(activeReplies);
        }

    public async Task<List<TweetResponseDto>> GetRepostsByTweetIdAsync(long id)
{
    var tweet = await _tweetRepository.GetTweetByIdAsync(id);
    if (tweet == null || tweet.Deleted) throw new KeyNotFoundException("Tweet not found");

    var reposts = await _tweetRepository.GetRepostsByTweetIdAsync(id);
    var activeReposts = reposts.Where(repost => !repost.Deleted).ToList();

    return _mapper.Map<List<TweetResponseDto>>(activeReposts);
}

public async Task<List<UserResponseDto>> GetMentionsByTweetIdAsync(long id)
    {
        var tweet = await _tweetRepository.GetTweetByIdAsync(id);
        if (tweet == null || tweet.Deleted) throw new KeyNotFoundException("Tweet not found");

        var mentionedUsers = await _tweetRepository.GetMentionsByTweetIdAsync(id);
        return _mapper.Map<List<UserResponseDto>>(mentionedUsers);
    }
    }
}

//      public async Task<List<UserResponseDto>> GetMentionsByTweetIdAsync(long id)
//     {
//         var tweet = await _tweetRepository.GetTweetByIdAsync(id);
//         if (tweet == null || tweet.Deleted) throw new KeyNotFoundException("Tweet not found");

//         var mentionedUsers = await _tweetRepository.GetMentionsByTweetIdAsync(id);
//         return _mapper.Map<List<UserResponseDto>>(mentionedUsers);
//     }
//     }
// }
// private List<string> ExtractMentions(string content)
// {
//     var mentions = new List<string>();
//     var matches = Regex.Matches(content, @"@\w+");
//     foreach (Match match in matches)
//     {
//         mentions.Add(match.Value.TrimStart('@'));
//     }
//     return mentions;
// }

// }
//}






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