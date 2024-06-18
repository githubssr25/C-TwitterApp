using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBackendApp.Entities
{
    public class Hashtag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Label { get; set; } = string.Empty; // Initialize

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FirstUsed { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUsed { get; set; }

        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>(); // Initialize
    }
}