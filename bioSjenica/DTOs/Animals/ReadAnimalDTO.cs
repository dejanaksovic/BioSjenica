using bioSjenica.Models;
using System.ComponentModel.DataAnnotations;

namespace bioSjenica.DTOs.AmnimalsDTO
{
    public class ReadAnimalDTO
    {
        public string RingNumber { get; set; }
        public string LatinicName { get; set; }
        public string CommonName { get; set; }
        public string ImageUrl { get; set; }
        public List<Region>? Regions { get; set; }
        public List<FeedingGround>? FeedingGrounds { get; set; }
    }
}
