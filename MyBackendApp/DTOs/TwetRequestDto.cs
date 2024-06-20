namespace MyBackendApp.DTOs
{
    public class TweetRequestDto
    {
        public string Content { get; set; } = string.Empty;
        public CredentialsDto Credentials { get; set; } = new CredentialsDto();
    }
}
