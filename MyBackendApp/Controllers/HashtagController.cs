using Microsoft.AspNetCore.Mvc;
using MyBackendApp.DTOs;
using MyBackendApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBackendApp.Controllers
{
    [Route("tags")]
    [ApiController]
    public class HashtagController : ControllerBase
    {
        private readonly IHashtagService _hashtagService;

        public HashtagController(IHashtagService hashtagService)
        {
            _hashtagService = hashtagService;
        }

        [HttpGet]
        public async Task<ActionResult<List<HashtagResponseDto>>> GetAllHashtags()
        {
            var hashtags = await _hashtagService.GetAllHashtagsAsync();
            return Ok(hashtags);
        }

        [HttpGet("{label}")]
        public async Task<ActionResult<List<TweetResponseDto>>> GetTweetsByHashtag(string label)
        {
            var tweets = await _hashtagService.GetTweetsByHashtagAsync(label);
            return Ok(tweets);
        }
    }
}
