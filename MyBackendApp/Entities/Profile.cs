using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MyBackendApp.Entities
{
    [Owned]
    public class Profile
    {
        [Required]
        public string FirstName { get; set; } = string.Empty; // Initialize

        [Required]
        public string LastName { get; set; } = string.Empty; // Initialize

        [Required]
        public string Email { get; set; } = string.Empty; // Initialize

        public string Phone { get; set; } = string.Empty; // Initialize
    }
}
