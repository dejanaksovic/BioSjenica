using bioSjenica.Models;

namespace bioSjenica.DTOs {
  public class ReadPlantDTO {
    public string LatinicName { get; set; }
    public string ImageUrl { get; set; }
    public string CommonName { get; set; }
    public List<Region>? Regions{ get; set; }
    public string? SpecialDecision { get; set; }
    public DateTime? SpecialTime { get; set; }
  }
}