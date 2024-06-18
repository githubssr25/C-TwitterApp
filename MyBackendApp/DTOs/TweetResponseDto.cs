using System;

namespace MyBackendApp.DTOs
{
    public class TweetResponseDto
    {
        public long Id { get; set; }
        public UserResponseDto Author { get; set; } = new UserResponseDto();
        public DateTime Posted { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
