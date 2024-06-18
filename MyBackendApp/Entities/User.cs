using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyBackendApp.Entities
{
    [Table("user_account")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public Credentials? Credentials { get; set; } // Make nullable

        [Required]
        public Profile? Profile { get; set; } // Make nullable

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Joined { get; set; }

        public bool Deleted { get; set; } = false;

        public ICollection<Tweet>? Tweets { get; set; } = new List<Tweet>(); // Initialize and make nullable

        public ICollection<User>? Following { get; set; } = new List<User>(); // Initialize and make nullable

        public ICollection<User>? Followers { get; set; } = new List<User>(); // Initialize and make nullable

        public ICollection<Tweet>? LikedTweets { get; set; } = new List<Tweet>(); // Initialize and make nullable

        public ICollection<Tweet>? MentionedTweets { get; set; } = new List<Tweet>(); // Initialize and make nullable
    }
}
