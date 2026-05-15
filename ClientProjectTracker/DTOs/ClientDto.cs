using System.ComponentModel.DataAnnotations;

namespace ClientProjectTracker.DTOs
{
    public class ClientDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? CompanyName { get; set; }
    }
}