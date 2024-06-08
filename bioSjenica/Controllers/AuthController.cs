using bioSjenica.DTOs;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Controllers {
  [Controller]
  [Route("/api/[controller]")]
  public class AuthController:ControllerBase {
    [HttpPost]
    public ActionResult<string> GetToken([FromBody]AuthDTO user) {
      var jwtToken = AuthHelper.GenerateJWTToken(user);
      return Ok(jwtToken);
    }  
  }
}