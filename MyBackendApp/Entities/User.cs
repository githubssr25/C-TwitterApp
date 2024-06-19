using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Entities
{
    [Table("user_account")] // Rename due to reserved keyword
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public Credentials Credentials { get; set; } = new Credentials();

        [Required]
        public Profile Profile { get; set; } = new Profile();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Joined { get; set; }

        public bool Deleted { get; set; } = false;

        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();

        public ICollection<User> Following { get; set; } = new List<User>();

        public ICollection<User> Followers { get; set; } = new List<User>();

        public ICollection<Tweet> LikedTweets { get; set; } = new List<Tweet>();

        public ICollection<Tweet> MentionedTweets { get; set; } = new List<Tweet>();
    }
}
