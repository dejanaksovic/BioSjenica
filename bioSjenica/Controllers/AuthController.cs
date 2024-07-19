using System.Diagnostics.Tracing;
using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Models;
using bioSjenica.Repositories;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace bioSjenica.Controllers {
  [Controller]
  [Route("/api/[controller]")]
  public class AuthController:ControllerBase {
    private readonly SqlContext _sqlContext;
    private readonly IAuthRepository _authRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILogger<AuthController> _logger;
    public AuthController(SqlContext sqlContext, IAuthRepository authRepo, IUserRepository userRepo, ILogger<AuthController> logger)
    {
      _logger = logger;
      _sqlContext = sqlContext;
      _authRepo = authRepo;
      _userRepo = userRepo;
    }
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User user) {

      var successUser = await _userRepo.Create(user);

      if(successUser is null) {
        throw new NotImplementedException();
      }
      return Ok(successUser);
    }

    [HttpPost]
    public async Task<ActionResult<string>> LoginUserPassword([FromBody]AuthRequestDTO userCreds) {
      //I just gave up, realized i should have put repositories so that they return actual models not mappers
      var user = await _sqlContext.Users.FirstOrDefaultAsync(u => u.Email == userCreds.Email);
      var resp = new HttpResponseMessage();
      if(user is null) {
        throw new NotImplementedException();
      }
      
      var accessToken = _authRepo.GetAccessToken(user);
      var refreshToken = _authRepo.GetRefreshToken(user);

      //save refreshToken in database
      user.RefreshToken = refreshToken;
      await _sqlContext.SaveChangesAsync();
      
      // Set refresh token inside a cookie
      Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions{
        Expires = DateTimeOffset.UtcNow.AddDays(7),
        HttpOnly = true,
        IsEssential = true,
        Secure = true,
        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
      });

      return Ok(accessToken);
    }  
  }
}