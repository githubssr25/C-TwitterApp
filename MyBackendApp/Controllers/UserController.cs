using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBackendApp.DTOs;
using MyBackendApp.Services;

namespace MyBackendApp.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
        {
            return await _userService.GetAllUsersAsync();
        }

        [HttpGet("{username}")]
        public async Task<UserResponseDto> GetUserByUsername(string username)
        {
            return await _userService.GetUserByUsernameAsync(username);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            var createdUser = await _userService.CreateUserAsync(userRequestDto);
            return CreatedAtAction(nameof(GetUserByUsername), new { username = createdUser.Username }, createdUser);
        }

        [HttpPost("{username}/follow")]
        public async Task<IActionResult> FollowUser(string username, [FromBody] CredentialsDto credentialsDto)
        {
            await _userService.FollowUserAsync(username, credentialsDto);
            return NoContent();
        }

        [HttpPost("{username}/unfollow")]
        public async Task<IActionResult> UnFollowUser(string username, [FromBody] CredentialsDto credentialsDto)
        {
            await _userService.UnFollowUserAsync(username, credentialsDto);
            return NoContent();
        }

        [HttpGet("{username}/tweets")]
        public async Task<IEnumerable<TweetResponseDto>> GetTweets(string username)
        {
            return await _userService.GetTweetsAsync(username);
        }

        [HttpGet("{username}/mentions")]
        public async Task<IEnumerable<TweetResponseDto>> GetMentions(string username)
        {
            return await _userService.GetMentionsAsync(username);
        }

        [HttpGet("{username}/followers")]
        public async Task<IEnumerable<UserResponseDto>> GetFollowers(string username)
        {
            return await _userService.GetFollowersAsync(username);
        }

        [HttpGet("{username}/following")]
        public async Task<IActionResult> GetFollowing(string username)
        {
            try
            {
                var following = await _userService.GetFollowingAsync(username);
                return Ok(following);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{username}/feed")]
        public async Task<IActionResult> GetFeed(string username)
        {
            var feed = await _userService.GetFeedAsync(username);
            return Ok(feed);
        }
    }
}
