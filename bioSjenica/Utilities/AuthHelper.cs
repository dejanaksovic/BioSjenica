using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bioSjenica.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace bioSjenica.Utilities {
  public static class AuthHelper {
    public static string GenerateJWTToken(ReadUserDTO user) {
      var claims = new List<Claim> {
        new Claim(ClaimTypes.NameIdentifier, user.SSN),
        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
        new Claim(ClaimTypes.Role, user.Role)
      };
      var jwtToken = new JwtSecurityToken(
        issuer: "api/biosjenica",
        audience: "biosjenica",
        claims: claims,
        notBefore: DateTime.UtcNow,
        expires: DateTime.UtcNow.AddMinutes(10),
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