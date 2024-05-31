using System.ComponentModel.DataAnnotations;

namespace bioSjenica.Models
{
    public class User
    {
        // Not gonna use DTOs since only user themselfs and adminst have the info
        [Key]
        [Required]
        public string SSN { get; set; }
        [Required]
        public string FistName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        public float? PayGrade { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
