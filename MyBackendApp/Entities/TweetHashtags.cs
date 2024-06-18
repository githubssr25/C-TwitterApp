using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Entities
{
    public class TweetHashtags
    {
        public long TweetId { get; set; }
        [ForeignKey("TweetId")]
        public Tweet Tweet { get; set; } = null!; // Marked as non-nullable

        public long HashtagId { get; set; }
        [ForeignKey("HashtagId")]
        public Hashtag Hashtag { get; set; } = null!; // Marked as non-nullable
    }
}