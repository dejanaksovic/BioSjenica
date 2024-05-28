namespace bioSjenica.DTOs {
  public class CreatePlantDTO {
        public string LatinicName { get; set; }
        public string ImageUrl { get; set; }
        public string CommonName { get; set; }
        public List<string> RegionNames{ get; set; }
        public string? SpecialDecision { get; set; }
        public DateTime? SpecialTime { get; set; }
  }
}