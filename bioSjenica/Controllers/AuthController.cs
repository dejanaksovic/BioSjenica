using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Repositories;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Controllers {
  [Controller]
  [Route("/api/[controller]")]
  public class AuthController:ControllerBase {
    private readonly SqlContext _sqlContext;
    private readonly IUserMapper _userMapper;
    public AuthController(SqlContext sqlContext, IUserMapper userMapper)
    {
      _sqlContext = sqlContext;
      _userMapper = userMapper;
    }
    [HttpPost]
    public async Task<ActionResult<string>> GetTokenFromUsernamePassword([FromBody]AuthDTO userPayload) {
      //I just gave up, realized i should have put repositories so that they return actual models not mappers
      var user = await _sqlContext.Users.FirstOrDefaultAsync(u => u.Email == userPayload.email);
      if(user is null) {
        throw new NotImplementedException();
      }

      var token = AuthHelper.GenerateJWTToken(_userMapper.UserToRead(user));

      return Ok(token);
    }  
  }
}