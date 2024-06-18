using Microsoft.EntityFrameworkCore;
using MyBackendApp.Entities;

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

            // Self-referencing many-to-many relationship for followers and following
            modelBuilder.Entity<User>()
                .HasMany(u => u.Following)
                .WithMany(u => u.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "UserUser",
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("FollowerId", "FollowingId");
                        j.ToTable("followers_following");
                    });

            // Many-to-many relationship for user likes
            modelBuilder.Entity<User>()
                .HasMany(u => u.LikedTweets)
                .WithMany(t => t.LikedByUsers)
                .UsingEntity(j => j.ToTable("user_likes"));

            // Many-to-many relationship for user mentions
            modelBuilder.Entity<User>()
                .HasMany(u => u.MentionedTweets)
                .WithMany(t => t.MentionedUsers)
                .UsingEntity(j => j.ToTable("user_mentions"));

            // Configure one-to-many relationship between Tweet and User
            modelBuilder.Entity<Tweet>()
                .HasOne(t => t.Author)
                .WithMany(u => u.Tweets)
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); // Optional: define delete behavior
        }
    }
}
