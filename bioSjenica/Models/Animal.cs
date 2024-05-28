using System.ComponentModel.DataAnnotations;

namespace bioSjenica.Models
{
    public class Animal
    {
        public int Id { get; set; }
        [Required]
        public string RingNumber { get; set; }
        [Required]
        public string LatinicName { get; set; }
        [Required]
        public string CommonName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Info { get; set; }
    }
}
