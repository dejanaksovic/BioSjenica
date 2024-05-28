using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace bioSjenica.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Region
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Area { get; set; }
        [Required]
        public string Villages { get; set; }
        public List<FeedingGround> FeedingGrounds { get; set; }
        public List<Plant> Plants { get; set; }
        public List<Animal> Animals { get; set; }
        public string ProtectionType { get; set; }
    }
}
