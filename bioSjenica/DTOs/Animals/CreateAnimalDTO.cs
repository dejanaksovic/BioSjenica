using bioSjenica.Models;

namespace bioSjenica.DTOs.AmnimalsDTO
{
    public class CreateAnimalDTO
    {
        public string RingNumber { get; set; }
        public string LatinicName { get; set; }
        public string CommonName { get; set; }
        public string ImageUrl { get; set; }
        public List<string>? RegionNames { get; set; }
        public List<int>? GroundNumbers { get; set; }
    }
}
