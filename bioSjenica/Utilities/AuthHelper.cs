using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bioSjenica.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace bioSjenica.Utilities {
  public static class AuthHelper {
    public static string GenerateJWTToken(AuthDTO user) {
      var claims = new List<Claim> {
        new Claim(ClaimTypes.NameIdentifier, user.username ?? ""),
        new Claim(ClaimTypes.Name, user.username),
      };
      var jwtToken = new JwtSecurityToken(
        claims: claims,
        notBefore: DateTime.UtcNow,
        expires: DateTime.UtcNow.AddDays(1),
        signingCredentials: new SigningCredentials(
          new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("wIquAT4ASbLxhax8lhQTWJn8kUDgRz7q")
          ),
          SecurityAlgorithms.HmacSha256
        )
      );

      return new JwtSecurityTokenHandler().WriteToken(jwtToken).ToString();
    }
  }
}