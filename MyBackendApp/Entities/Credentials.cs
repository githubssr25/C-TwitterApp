using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyBackendApp.Entities
{
    [Owned]
    public class Credentials
    {
        [Required]
        public string Username { get; set; } = string.Empty; // Initialize

        [Required]
        public string Password { get; set; } = string.Empty; // Initialize
    }
}
