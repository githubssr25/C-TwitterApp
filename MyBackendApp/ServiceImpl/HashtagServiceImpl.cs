using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MyBackendApp.DTOs;
using MyBackendApp.Repositories;

namespace MyBackendApp.Services
{
    public class HashtagServiceImpl : IHashtagService
    {
        private readonly IHashtagRepository _hashtagRepository;
        private readonly IMapper _mapper;

        public HashtagServiceImpl(IHashtagRepository hashtagRepository, IMapper mapper)
        {
            _hashtagRepository = hashtagRepository;
            _mapper = mapper;
        }

        public async Task<List<HashtagResponseDto>> GetAllHashtagsAsync()
        {
            var hashtags = await _hashtagRepository.GetAllHashtagsAsync();
            return _mapper.Map<List<HashtagResponseDto>>(hashtags);
        }

         public async Task<List<TweetResponseDto>> GetTweetsByHashtagAsync(string label)
        {
            // Normalize the label to include the "#" prefix if not already present
            string normalizedLabel = label.StartsWith("#") ? label : "#" + label;

            var hashtag = await _hashtagRepository.GetHashtagByLabelAsync(normalizedLabel);
            if (hashtag == null)
            {
                throw new KeyNotFoundException("Hashtag not found");
            }

            // Filter tweets to exclude deleted ones and sort by posted date
            var tweets = hashtag.Tweets
                .Where(tweet => !tweet.Deleted)
                .OrderByDescending(tweet => tweet.Posted)
                .ToList();

            return _mapper.Map<List<TweetResponseDto>>(tweets);
        }

        public async Task<bool> CheckHashtagExistsAsync(string label)
        {
            string normalizedLabel = label.StartsWith("#") ? label : "#" + label;
            return await _hashtagRepository.CheckHashtagExistsAsync(normalizedLabel);
        }
    }
    }
