using bioSjenica.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bioSjenica.DTOs
{
    public class FeedingGroundDTO
    {
        public int GroundNumber { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public DateTime StartWork { get; set; }
        public DateTime EndWork { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
