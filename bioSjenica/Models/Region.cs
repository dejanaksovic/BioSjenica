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
        // Decided on a composite string type[village1, villag2, villag3] instead of not needed village instance, since only the names are tracked
        public string Villages { get; set; }
        public string ProtectionType { get; set; }
        public List<FeedingGround>? FeedingGrounds { get; set; }
        public List<Plant>? Plants { get; set; }
        public List<Animal>? Animals { get; set; }
    }
}
