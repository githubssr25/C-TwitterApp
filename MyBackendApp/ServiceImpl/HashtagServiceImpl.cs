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
            var tweets = await _hashtagRepository.GetTweetsByHashtagAsync(label);
            if (tweets == null || tweets.Count == 0)
            {
                throw new KeyNotFoundException("Hashtag not found");
            }

            return _mapper.Map<List<TweetResponseDto>>(tweets);
        }

        public async Task<bool> CheckHashtagExistsAsync(string label)
        {
            return await _hashtagRepository.CheckHashtagExistsAsync(label);
        }
    }
}
