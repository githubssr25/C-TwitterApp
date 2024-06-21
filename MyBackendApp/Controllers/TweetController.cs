using Microsoft.AspNetCore.Mvc;
using MyBackendApp.DTOs;
using MyBackendApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;


namespace MyBackendApp.Controllers
{
    [Route("tweets")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService _tweetService;
        private readonly ILogger<TweetController> _logger;

        public TweetController(ITweetService tweetService, ILogger<TweetController> logger)
        {
            _tweetService = tweetService;
            _logger = logger;
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

        [HttpPost("{id}/reply")]
        public async Task<ActionResult<TweetResponseDto>> ReplyToTweet(long id, [FromBody] TweetRequestDto tweetRequestDto)
        {
            try
            {
                var tweet = await _tweetService.ReplyToTweetAsync(id, tweetRequestDto);
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

       [HttpPost("{id}/repost")]
        public async Task<ActionResult<TweetResponseDto>> RepostTweet(long id)
        {
            string body;
            using (var reader = new System.IO.StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
                _logger.LogInformation("Received request body: {Body}", body);
            }

            if (string.IsNullOrEmpty(body))
            {
                _logger.LogWarning("Request body is empty.");
                return BadRequest("Invalid request body.");
            }

            CredentialsDto credentialsDto;
            try
            {
                credentialsDto = JsonSerializer.Deserialize<CredentialsDto>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing request body.");
                return BadRequest("Invalid request body.");
            }

            // Log the received credentials object
            LogObject(credentialsDto);

            if (credentialsDto == null)
            {
                _logger.LogWarning("CredentialsDto is null.");
                return BadRequest("Invalid request body.");
            }

            _logger.LogInformation("Received repost request with ID: {Id} and credentials: Username: {Username}, Password: {Password}", id, credentialsDto?.Username, credentialsDto?.Password);

            try
            {
                var tweet = await _tweetService.RepostTweetAsync(id, credentialsDto);
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

        private void LogObject(object obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(obj, options);
            _logger.LogInformation($"Serialized Object: {jsonString}");
        }

        [HttpGet("{id}/tags")]
        public async Task<ActionResult<List<HashtagResponseDto>>> GetTagsByTweetId(long id)
        {
            try
            {
                var tags = await _tweetService.GetTagsByTweetIdAsync(id);
                return Ok(tags);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/likes")]
        public async Task<ActionResult<List<UserResponseDto>>> GetLikesByTweetId(long id)
        {
            try
            {
                var likes = await _tweetService.GetLikesByTweetIdAsync(id);
                return Ok(likes);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/context")]
        public async Task<ActionResult<List<TweetResponseDto>>> GetTweetContextById(long id)
        {
            try
            {
                var context = await _tweetService.GetTweetContextByIdAsync(id);
                return Ok(context);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/replies")]
        public async Task<ActionResult<List<TweetResponseDto>>> GetRepliesByTweetId(long id)
        {
            try
            {
                var replies = await _tweetService.GetRepliesByTweetIdAsync(id);
                return Ok(replies);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/reposts")]
        public async Task<ActionResult<List<TweetResponseDto>>> GetRepostsByTweetId(long id)
        {
            try
            {
                var reposts = await _tweetService.GetRepostsByTweetIdAsync(id);
                return Ok(reposts);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/mentions")]
        public async Task<ActionResult<List<UserResponseDto>>> GetMentionsByTweetId(long id)
        {
            try
            {
                var mentions = await _tweetService.GetMentionsByTweetIdAsync(id);
                return Ok(mentions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}