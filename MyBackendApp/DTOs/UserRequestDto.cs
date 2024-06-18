using System.ComponentModel.DataAnnotations;

namespace MyBackendApp.DTOs
{
    public class UserRequestDto
    {
        public ProfileDto Profile { get; set; } = new ProfileDto();
        public CredentialsDto Credentials { get; set; } = new CredentialsDto();
    }
}

