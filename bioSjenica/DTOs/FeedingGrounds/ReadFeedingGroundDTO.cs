using bioSjenica.Models;

namespace bioSjenica.DTOs {
  public class ReadFeedingGroundDTO {
    public int GroundNumber { get; set; }
    public Region Region { get; set; }
    public DateTime? StartWork { get; set; }
    public DateTime? EndWork { get; set; }
    public List<Animal>? Animals { get; set; }
  }
}