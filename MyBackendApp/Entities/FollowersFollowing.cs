using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Entities
{
    public class FollowersFollowing
    {
        public long FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public User Follower { get; set; } = null!; // Marked as non-nullable

        public long FollowingId { get; set; }
        [ForeignKey("FollowingId")]
        public User Following { get; set; } = null!; // Marked as non-nullable
    }
}