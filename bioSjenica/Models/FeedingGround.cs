using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bioSjenica.Models
{
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
        public DateTime StartWork { get; set; }
        [Required]
        public DateTime EndWork { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
