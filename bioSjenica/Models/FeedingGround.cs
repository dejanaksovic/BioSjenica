using System.ComponentModel.DataAnnotations;

namespace bioSjenica.Models
{
    public class FeedingGround
    {
        public int Id { get; set; }
        [Required]
        public int GroundNumber { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public DateTime StartWork { get; set; }
        [Required]
        public DateTime EndWork { get; set; }

    }
}
