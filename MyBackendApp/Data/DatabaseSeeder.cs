using System;
using System.Collections.Generic;
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
                    new Hashtag { Label = "#eldenlord", FirstUsed = now, LastUsed = now },
                    new Hashtag { Label = "#mario", FirstUsed = now, LastUsed = now },
                    new Hashtag { Label = "#luigi", FirstUsed = now, LastUsed = now },
                    new Hashtag { Label = "#whereiscortana", FirstUsed = now, LastUsed = now }
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
                    MentionedUsers = new List<User> { users[0], users[1] }
                };
                context.Tweets.Add(tweet1);
                context.SaveChanges();

                var tweet2 = new Tweet
                {
                    Author = users[0],
                    Content = "This is some content 2 tweet2 #eldenlord #mario",
                    Deleted = false,
                    Hashtags = new List<Hashtag> { hashtags[0], hashtags[1] },
                    InReplyTo = tweet1
                };
                context.Tweets.Add(tweet2);
                context.SaveChanges();

                var tweet3 = new Tweet
                {
                    Author = users[1],
                    Content = "This is some content 3 tweet3 #luigi #whereiscortana",
                    Deleted = false,
                    Hashtags = new List<Hashtag> { hashtags[2], hashtags[3] },
                    InReplyTo = tweet2
                };
                context.Tweets.Add(tweet3);
                context.SaveChanges();

                var tweet4 = new Tweet
                {
                    Author = users[1],
                    Content = "This is some content 4 tweet4",
                    Deleted = false,
                    InReplyTo = tweet3
                };
                context.Tweets.Add(tweet4);
                context.SaveChanges();

                var tweet5 = new Tweet
                {
                    Author = users[2],
                    Content = "This is some content 5 tweet5",
                    Deleted = false,
                    MentionedUsers = new List<User> { users[0], users[1] },
                    InReplyTo = tweet4
                };
                context.Tweets.Add(tweet5);
                context.SaveChanges();

                var tweet6 = new Tweet
                {
                    Author = users[2],
                    Deleted = false,
                    RepostOf = tweet5,
                    MentionedUsers = new List<User> { users[0], users[1] },
                    InReplyTo = tweet2
                };
                context.Tweets.Add(tweet6);
                context.SaveChanges();

                var deletedTweet = new Tweet
                {
                    Author = users[2],
                    Content = "This is a deleted tweet (User3) tweet7",
                    Deleted = true,
                    MentionedUsers = new List<User> { users[0], users[1] }
                };
                context.Tweets.Add(deletedTweet);
                context.SaveChanges();

                var tweets = new List<Tweet> { tweet1, tweet2, tweet3, tweet4, tweet5, tweet6, deletedTweet };

                // Assign tweets to users
                users[0].Tweets = new List<Tweet> { tweet1, tweet2 };
                users[1].Tweets = new List<Tweet> { tweet3, tweet4 };
                users[2].Tweets = new List<Tweet> { tweet5, tweet6 };

                context.Users.UpdateRange(users);
                context.SaveChanges();

                // --- List of Liked Tweets ---
                users[0].LikedTweets = new List<Tweet> { tweet5, tweet6 };
                users[1].LikedTweets = new List<Tweet> { tweet1, tweet2 };
                users[2].LikedTweets = new List<Tweet> { tweet3, tweet4 };
                users[5].LikedTweets = new List<Tweet> { tweet3, tweet4 };

                context.Users.UpdateRange(users);
                context.SaveChanges();

                // --- List of Following ---
                users[0].Following = new List<User> { users[1], users[2], users[3] };
                users[0].Followers = new List<User> { users[2], users[4] };

                context.Users.UpdateRange(users);
                context.SaveChanges();

                // --- Tweet Mentions ---
                var mention1 = new Tweet
                {
                    Author = users[1],
                    Content = "This is some content for tweet mention 1",
                    Deleted = false
                };

                context.Tweets.Add(mention1);
                context.SaveChanges();

                // Following
                users[0].Following = new List<User> { users[1], users[2], users[3], users[5] };
                users[0].Followers = new List<User> { users[4], users[5] };

                context.SaveChanges();
            }
        }
    }
}
