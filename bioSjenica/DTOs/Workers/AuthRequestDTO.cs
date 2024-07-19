using System.ComponentModel.DataAnnotations;

public class AuthRequestDTO {
  [Required(ErrorMessage = "Email is required")]
  public string? Email { get; set; }
  [Required(ErrorMessage = "Password is required")]
  public string? Password { get; set; }
}