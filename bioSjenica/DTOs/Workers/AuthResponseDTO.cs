public class AuthResponseDTO {
  public bool IsAuthSuccess { get; set; }
  public string? ErrorMessage { get; set; }
  public string? AccessToken { get; set; }
}