namespace bioSjenica.DTOs {
  public class ReadUserDTO {
        public string? SSN { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public float? PayGrade { get; set; } = 0;
        public string? Role { get; set; } = "User";
        public string? Email { get; set; }
  }
}