using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Models
{
    [Index(nameof(SSN), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        // Not gonna use DTOs since only user themselfs and adminst have the info
        [Key]
        [Required]
        public string SSN { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? Address { get; set; }
        [Required]
        public float PayGrade { get; set; } = 0;
        [Required]
        public string Role { get; set; } = "User";
        [Required]
        public string Email { get; set; }
        public string? Password { get; set; } = "";
    }
}
