using Microsoft.AspNetCore.Mvc;
using MyBackendApp.DTOs;
using MyBackendApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBackendApp.Controllers
{
    [Route("tweets")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService _tweetService;

        public TweetController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TweetResponseDto>>> GetAllTweets()
        {
            var tweets = await _tweetService.GetAllTweetsAsync();
            return Ok(tweets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TweetResponseDto>> GetTweetById(long id)
        {
            try
            {
                var tweet = await _tweetService.GetTweetByIdAsync(id);
                return Ok(tweet);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TweetResponseDto>> CreateTweet([FromBody] TweetRequestDto tweetRequestDto)
        {
            try
            {
                var tweet = await _tweetService.CreateTweetAsync(tweetRequestDto);
                return CreatedAtAction(nameof(GetTweetById), new { id = tweet.Id }, tweet);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TweetResponseDto>> DeleteTweet(long id, [FromBody] CredentialsDto credentialsDto)
        {
            try
            {
                var tweet = await _tweetService.DeleteTweetAsync(id, credentialsDto);
                return Ok(tweet);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikeTweet(long id, [FromBody] CredentialsDto credentialsDto)
        {
            try
            {
                await _tweetService.LikeTweetAsync(id, credentialsDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
