using bioSjenica.Models;

public interface IAuthRepository {
  public string GetRefreshToken(User user);
  public string GetAccessToken(User user);
}