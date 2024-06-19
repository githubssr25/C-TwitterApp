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

        





    }
}
