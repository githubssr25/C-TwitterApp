using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Entities
{
    public class UserMentions
    {
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!; // Marked as non-nullable

        public long TweetId { get; set; }
        [ForeignKey("TweetId")]
        public Tweet Tweet { get; set; } = null!; // Marked as non-nullable
    }
}