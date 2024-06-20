using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBackendApp.Data;
using MyBackendApp.Entities;

namespace MyBackendApp.Seeders
{
    public static class DatabaseSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return; // DB has been seeded
                }

                // --- Users ---
                var users = new List<User>
                {
                    new User
                    {
                        Credentials = new Credentials { Username = "therealmc", Password = "Password" },
                        Profile = new Profile { FirstName = "Master", LastName = "Chief", Email = "sierra117@email.com", Phone = "123-456-7890" },
                        Joined = DateTime.UtcNow,
                        Deleted = false
                    },
                    new User
                    {
                        Credentials = new Credentials { Username = "mario", Password = "password" },
                        Profile = new Profile { FirstName = "Mario", LastName = "Mario", Email = "mario@email.com", Phone = "234-567-8901" },
                        Joined = DateTime.UtcNow,
                        Deleted = false
                    },
                    new User
                    {
                        Credentials = new Credentials { Username = "Luigi", Password = "Password" },
                        Profile = new Profile { FirstName = "Luigi", LastName = "Mario", Email = "luigi@email.com", Phone = "345-678-9012" },
                        Joined = DateTime.UtcNow,
                        Deleted = false
                    },
                    new User
                    {
                        Credentials = new Credentials { Username = "Nathan", Password = "Password" },
                        Profile = new Profile { FirstName = "Nathan", LastName = "Drake", Email = "nathan@email.com", Phone = "456-789-0023" },
                        Joined = DateTime.UtcNow,
                        Deleted = false
                    },
                    new User
                    {
                        Credentials = new Credentials { Username = "Tarnished", Password = "Password" },
                        Profile = new Profile { FirstName = "The", LastName = "Tarnished", Email = "willibecometheeldenlord@email.com", Phone = "567-890-0034" },
                        Joined = DateTime.UtcNow,
                        Deleted = false
                    },
                    new User
                    {
                        Credentials = new Credentials { Username = "DeletedUser", Password = "Password" },
                        Profile = new Profile { FirstName = "Deleted", LastName = "User", Email = "Deleted@User.com", Phone = "NULL" },
                        Joined = DateTime.UtcNow,
                        Deleted = true
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();

                // --- Hashtags ---
                var now = DateTime.UtcNow;
                var hashtags = new List<Hashtag>
                {
                    new Hashtag { Id = 2, Label = "#eldenlord", FirstUsed = now, LastUsed = now },
                    new Hashtag { Id = 3, Label = "#mario", FirstUsed = now, LastUsed = now },
                    new Hashtag { Id = 4, Label = "#luigi", FirstUsed = now, LastUsed = now },
                    new Hashtag { Id = 5, Label = "#whereiscortana", FirstUsed = now, LastUsed = now }
                };

                context.Hashtags.AddRange(hashtags);
                context.SaveChanges();

                // --- Tweets ---
                var tweet1 = new Tweet
                {
                    Author = users[0],
                    Content = "This is some content 1 tweet1 #eldenlord #mario",
                    Deleted = false,
                    Hashtags = new List<Hashtag> { hashtags[0], hashtags[1] },
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet1);
                context.SaveChanges();

                var tweet2 = new Tweet
                {
                    Author = users[0],
                    Content = "This is some content 2 tweet2 #eldenlord #mario",
                    Deleted = false,
                    Hashtags = new List<Hashtag> { hashtags[0], hashtags[1] },
                    InReplyTo = tweet1,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet2);
                context.SaveChanges();

                var tweet3 = new Tweet
                {
                    Author = users[1],
                    Content = "This is some content 3 tweet3 #luigi #whereiscortana",
                    Deleted = false,
                    Hashtags = new List<Hashtag> { hashtags[2], hashtags[3] },
                    InReplyTo = tweet2,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet3);
                context.SaveChanges();

                var tweet4 = new Tweet
                {
                    Author = users[1],
                    Content = "This is some content 4 tweet4",
                    Deleted = false,
                    InReplyTo = tweet3,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet4);
                context.SaveChanges();

                var tweet5 = new Tweet
                {
                    Author = users[2],
                    Content = "This is some content 5 tweet5",
                    Deleted = false,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet5);
                context.SaveChanges();

                var tweet6 = new Tweet
                {
                    Author = users[2],
                    Content = "This is a repost of tweet5",
                    Deleted = false,
                    RepostOf = tweet5,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet6);
                context.SaveChanges();

                var tweet7 = new Tweet
                {
                    Author = users[2],
                    Content = "This is a deleted tweet",
                    Deleted = true,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet7);
                context.SaveChanges();

                // New tweets as per provided queries
                var tweet8 = new Tweet
                {
                    Author = users[1],
                    Content = "This is a reply to tweet1",
                    Deleted = false,
                    InReplyTo = tweet1,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet8);
                context.SaveChanges();

                var tweet9 = new Tweet
                {
                    Author = users[1],
                    Content = "This is a reply to tweet2",
                    Deleted = false,
                    InReplyTo = tweet2,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet9);
                context.SaveChanges();

                var tweet10 = new Tweet
                {
                    Author = users[1],
                    Content = "This is a reply to tweet3",
                    Deleted = false,
                    InReplyTo = tweet3,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet10);
                context.SaveChanges();

                var tweet11 = new Tweet
                {
                    Author = users[3],
                    Content = "This is a repost of tweet1",
                    Deleted = false,
                    RepostOf = tweet1,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet11);
                context.SaveChanges();

                var tweet12 = new Tweet
                {
                    Author = users[3],
                    Content = "This is a repost of tweet2",
                    Deleted = false,
                    RepostOf = tweet2,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet12);
                context.SaveChanges();

                var tweet13 = new Tweet
                {
                    Author = users[4],
                    Content = "This is a repost of tweet3",
                    Deleted = false,
                    RepostOf = tweet3,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet13);
                context.SaveChanges();

                var tweet14 = new Tweet
                {
                    Author = users[1],
                    Content = "This is a reply and repost of tweet1",
                    Deleted = false,
                    InReplyTo = tweet1,
                    RepostOf = tweet1,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet14);
                context.SaveChanges();

                var tweet15 = new Tweet
                {
                    Author = users[4],
                    Content = "This is a reply and repost of tweet2",
                    Deleted = false,
                    InReplyTo = tweet2,
                    RepostOf = tweet2,
                    Posted = DateTime.UtcNow
                };
                context.Tweets.Add(tweet15);
                context.SaveChanges();

                var tweets = new List<Tweet> { tweet1, tweet2, tweet3, tweet4, tweet5, tweet6, tweet7, tweet8, tweet9, tweet10, tweet11, tweet12, tweet13, tweet14, tweet15 };

                // Assign tweets to users
                users[0].Tweets = new List<Tweet> { tweet1, tweet2 };
                users[1].Tweets = new List<Tweet> { tweet3, tweet4, tweet8, tweet9, tweet10, tweet14 };
                users[2].Tweets = new List<Tweet> { tweet5, tweet6, tweet7 };
                users[3].Tweets = new List<Tweet> { tweet11, tweet12 };
                users[4].Tweets = new List<Tweet> { tweet13, tweet15 };

                context.Users.UpdateRange(users);
                context.SaveChanges();

                // --- List of Liked Tweets ---
                users[1].LikedTweets = new List<Tweet> { tweet5, tweet6 };
                users[2].LikedTweets = new List<Tweet> { tweet1, tweet2 };
                users[3].LikedTweets = new List<Tweet> { tweet3, tweet4 };
                users[6].LikedTweets = new List<Tweet> { tweet3, tweet4 };

                context.Users.UpdateRange(users);
                context.SaveChanges();

                // --- List of Following i edited this becasue there is no userID1 have to shift up prob why some of tehse seeder values dont work---
                users[1].Following = new List<User> { users[2], users[3], users[4], users[6] };
                users[1].Followers = new List<User> { users[5], users[6] };
                users[2].Following = new List<User> { users[3] };
                users[3].Following = new List<User> { users[1] };
                users[4].Following = new List<User> { users[1], users[2] };
                users[5].Following = new List<User> { users[1] };


                context.Users.UpdateRange(users);
                context.SaveChanges();

                // --- Tweet Mentions ---
                context.Database.ExecuteSqlRaw(
                    @"INSERT INTO user_mentions (""MentionedTweetsId"", ""MentionedUsersId"") VALUES
                    (1, 2), -- tweet 1 mentions therealmc
                    (1, 3), -- tweet 1 mentions mario
                    (2, 2), -- tweet 2 mentions therealmc
                    (3, 3), -- tweet 3 mentions mario
                    (4, 4), -- tweet 4 mentions Nathan
                    (5, 5), -- tweet 5 mentions Tarnished
                    (6, 2), -- tweet 6 mentions therealmc
                    (7, 3); -- tweet 7 mentions mario");

                // --- Hashtag-Tweet Relationships ---
                context.Database.ExecuteSqlRaw(
                    @"INSERT INTO HashtagTweet (""HashtagsId"", ""TweetsId"") VALUES
                    (2, 1), -- #eldenlord in tweet1
                    (2, 2), -- #eldenlord in tweet2
                    (3, 1), -- #mario in tweet1
                    (3, 2), -- #mario in tweet2
                    (4, 3), -- #luigi in tweet3
                    (5, 3); -- #whereiscortana in tweet3");

                context.SaveChanges();
            }
        }
    }
}
