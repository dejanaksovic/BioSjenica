namespace bioSjenica.DTOs {
  public class CreateFeedingGroundDTO {
        public int? GroundNumber { get; set; } = null;
        public string? RegionName { get; set; }
        public int? StartWork { get; set; }
        public int? EndWork { get; set; }
        public List<string>? AnimalsLatinicOrCommonName { get; set; }
  }
}