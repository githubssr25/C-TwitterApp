using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Entities
{
    public class Tweet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

       [Required]
        public User Author { get; set; } = null!; // Marked as non-nullable

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Posted { get; set; }

        public bool Deleted { get; set; } = false;

        
        [Required]
        public string Content { get; set; } = string.Empty; // Initialize

        public Tweet? InReplyTo { get; set; } // Make nullable

        public ICollection<Tweet>? Replies { get; set; } = new List<Tweet>(); // Initialize and make nullable

        public Tweet? RepostOf { get; set; } // Make nullable

        public ICollection<Tweet>? Reposts { get; set; } = new List<Tweet>(); // Initialize and make nullable

        public ICollection<Hashtag>? Hashtags { get; set; } = new List<Hashtag>(); // Initialize and make nullable

        public ICollection<User>? MentionedUsers { get; set; } = new List<User>(); // Initialize and make nullable

        public ICollection<User>? LikedByUsers { get; set; } = new List<User>(); // Add and initialize
    }
}
