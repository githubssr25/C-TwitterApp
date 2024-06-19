using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Entities
{
    [Table("tweet")]
    public class Tweet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public bool Deleted { get; set; } = false;

        public long? InReplyToId { get; set; }
        [ForeignKey(nameof(InReplyToId))]
        public Tweet? InReplyTo { get; set; }

        public long? RepostOfId { get; set; }
        [ForeignKey(nameof(RepostOfId))]
        public Tweet? RepostOf { get; set; }

        public long? AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public User? Author { get; set; }

        public ICollection<Hashtag> Hashtags { get; set; } = new List<Hashtag>();

        public ICollection<User> MentionedUsers { get; set; } = new List<User>();

        public ICollection<User> LikedByUsers { get; set; } = new List<User>();

        [Required]
        public DateTime Posted { get; set; } // Add this property
    }
}
