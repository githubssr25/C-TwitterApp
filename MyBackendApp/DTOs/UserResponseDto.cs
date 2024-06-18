using System;

namespace MyBackendApp.DTOs
{
    public class UserResponseDto
    {
        public string Username { get; set; } = string.Empty;
        public ProfileDto Profile { get; set; } = new ProfileDto();
        public DateTime Joined { get; set; }
    }
}