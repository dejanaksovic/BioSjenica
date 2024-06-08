using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace bioSjenica.Models
{
    // Unique keys
    [Index(nameof(LatinicName), IsUnique = true)]
    [Index(nameof(CommonName), IsUnique = true)]
    [Index(nameof(ImageUrl), IsUnique = true)]
    public class Plant
    {
        public int Id { get; set; }
        [Required]
        public string LatinicName { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string CommonName { get; set; }
        [Required]
        public List<Region>? Regions{ get; set; }
        public string? SpecialDecision { get; set; }
        public DateTime? SpecialTime { get; set; }
    }
}
