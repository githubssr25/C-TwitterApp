using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Entities
{
    [Table("Hashtags")]
    public class Hashtag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Label { get; set; }

        [Required]
        public DateTime FirstUsed { get; set; }

        [Required]
        public DateTime LastUsed { get; set; }

        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
    }
}