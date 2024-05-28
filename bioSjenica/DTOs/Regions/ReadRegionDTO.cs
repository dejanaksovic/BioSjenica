using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Models;

namespace bioSjenica.DTOs.Regions
{
    public class ReadRegionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Area { get; set; }
        public string Villages { get; set; }
        public List<FeedingGround>? FeedingGrounds { get; set; }
        public List<Plant>? Plants { get; set; }
        public List<ReadAnimalDTO>? Animals { get; set; }
        public string ProtectionType { get; set; }
    }
}
