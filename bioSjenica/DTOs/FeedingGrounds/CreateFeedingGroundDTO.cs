namespace bioSjenica.DTOs {
  public class CreateFeedingGroundDTO {
        public int? GroundNumber { get; set; } = null;
        public string? RegionName { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? EndWork { get; set; }
        public List<string>? AnimalsLatinicOrCommonName { get; set; }
  }
}