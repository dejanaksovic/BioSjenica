
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bioSjenica.Models;
using Microsoft.IdentityModel.Tokens;

public class AuthRepository : IAuthRepository
{
    private readonly IConfiguration _config;
    private readonly IConfigurationSection _jwt;
    public AuthRepository(IConfiguration config)
    {
      _config = config;
      _jwt = _config.GetSection("JWT");
    }
    public string GetAccessToken(User user)
    {
        // Signing credentials
        var key = Encoding.UTF8.GetBytes(_jwt["Secret"]);
        var secret = new SymmetricSecurityKey(key);

        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        // Claims
        List<Claim> claims = new() {
          new Claim(ClaimTypes.Name, $"{user.FirstName} ${user.LastName}"),
          new Claim(ClaimTypes.Role, user.Role)
        };

        // Token option
        var options = new JwtSecurityToken(
          issuer: _jwt["Issuer"],
          audience: _jwt["Audience"],
          claims: claims,
          expires: DateTime.Now.AddMinutes(5),
          signingCredentials: credentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(options);

        return token;
    }

    public string GetRefreshToken(User user)
    {
        // Signing credentials
        var key = Encoding.UTF8.GetBytes(_jwt["Secret"]);
        var secret = new SymmetricSecurityKey(key);

        var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        // Claims
        List<Claim> claims = new() {
          new Claim(ClaimTypes.Name, $"{user.FirstName} ${user.LastName}"),
          new Claim(ClaimTypes.Role, user.Role)
        };

        // Token option
        var options = new JwtSecurityToken(
          issuer: _jwt["Issuer"],
          audience: _jwt["Audience"],
          claims: claims,
          expires: DateTime.Now.AddDays(7),
          signingCredentials: credentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(options);

        return token;
    }
}