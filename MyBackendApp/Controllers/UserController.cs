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

         private readonly ILogger<TweetServiceImpl> _logger;

        public UserController(IUserService userService, ILogger<TweetServiceImpl> logger)
        {
            _userService = userService;
            _logger = logger;
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


 [HttpDelete("{username}")]
public async Task<IActionResult> DeleteUser(string username, [FromBody] CredentialsDto credentialsDto)
{
    _logger.LogInformation("6-21 deleteUser request received with username: {username} and credentials: Username: {Username}, Password: {Password}", username, credentialsDto?.Username, credentialsDto?.Password);

    if (credentialsDto == null)
    {
        _logger.LogWarning("CredentialsDto is null.");
        return BadRequest("Invalid request body.");
    }

    try
    {
        var deletedUser = await _userService.DeleteUserAsync(username, credentialsDto);
        if (deletedUser == null)
            return NotFound("User not found or already deleted.");

        return Ok(deletedUser);
    }
    catch (UnauthorizedAccessException)
    {
        return Unauthorized("Invalid credentials.");
    }
    catch (KeyNotFoundException)
    {
        return NotFound("User not found.");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}

        [HttpPatch("{username}")]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UserRequestDto userRequestDto)
        {
            _logger.LogInformation("6-21 updateUser request received with username: {username} and credentials: Username: {Username}, Password: {Password}", username, userRequestDto?.Credentials?.Username, userRequestDto?.Credentials?.Password);

            if (userRequestDto == null || userRequestDto.Credentials == null || userRequestDto.Profile == null)
            {
                _logger.LogWarning("Invalid request body.");
                return BadRequest("Invalid request body.");
            }

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(username, userRequestDto.Credentials, userRequestDto.Profile);
                return Ok(updatedUser);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
