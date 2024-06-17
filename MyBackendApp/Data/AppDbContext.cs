using Microsoft.EntityFrameworkCore;

namespace MyBackendApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<FollowersFollowing> FollowersFollowing { get; set; }
        public DbSet<UserLikes> UserLikes { get; set; }
        public DbSet<UserMentions> UserMentions { get; set; }
        public DbSet<TweetHashtags> TweetHashtags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define composite key for FollowersFollowing
            modelBuilder.Entity<FollowersFollowing>()
                .HasKey(ff => new { ff.FollowerId, ff.FollowingId });

            modelBuilder.Entity<UserLikes>()
                .HasKey(ul => new { ul.UserId, ul.TweetId });

            modelBuilder.Entity<UserMentions>()
                .HasKey(um => new { um.UserId, um.TweetId });

            modelBuilder.Entity<TweetHashtags>()
                .HasKey(th => new { th.TweetId, th.HashtagId });
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        // Other properties...
    }

    public class Tweet
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        // Other properties...
    }

    public class Hashtag
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;
        // Other properties...
    }

    public class FollowersFollowing
    {
        public int FollowerId { get; set; }
        public User Follower { get; set; } = new User();
        public int FollowingId { get; set; }
        public User Following { get; set; } = new User();
    }

    public class UserLikes
    {
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; } = new Tweet();
    }

    public class UserMentions
    {
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; } = new Tweet();
    }

    public class TweetHashtags
    {
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; } = new Tweet();
        public int HashtagId { get; set; }
        public Hashtag Hashtag { get; set; } = new Hashtag();
    }
}
