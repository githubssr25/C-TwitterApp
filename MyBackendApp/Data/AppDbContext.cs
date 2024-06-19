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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-many relationship for followers
            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithMany(u => u.Following)
                .UsingEntity<Dictionary<string, object>>(
                    "followers_following",
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
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
