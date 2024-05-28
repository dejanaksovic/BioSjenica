using System.ComponentModel.DataAnnotations;

namespace bioSjenica.Models
{
    public class Worker
    {
        [Key]
        [Required]
        public string SSN { get; set; }
        [Required]
        public string FistName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public float PayGrade { get; set; }
    }
}
