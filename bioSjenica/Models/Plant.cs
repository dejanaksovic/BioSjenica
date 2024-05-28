using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace bioSjenica.Models
{
    [Index(nameof(LatinicName), nameof(CommonName), nameof(ImageUrl), IsUnique = true)]
    public class Plant
    {
        public int Id { get; set; }
        [Required]
        public string LatinicName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string CommonName { get; set; }
        public string? SpecialDecision { get; set; }
        public DateTime? SpecialTime { get; set; }
    }
}
