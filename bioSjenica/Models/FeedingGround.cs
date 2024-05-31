using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Models
{
    [Index(nameof(GroundNumber), IsUnique = true)]
    public class FeedingGround
    {
        public int Id { get; set; }
        [Required]
        public int GroundNumber { get; set; }
        [Required]
        [ForeignKey("Region")]
        public int RegionId { get; set; }
        public Region Region { get; set; }
        [Required]
        public int StartWork { get; set; }
        [Required]
        public int EndWork { get; set; }
        public List<Animal>? Animals { get; set; }
    }
}
