using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ClientProjectTracker.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? CompanyName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? OwnerId { get; set; }
        public IdentityUser? Owner { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}