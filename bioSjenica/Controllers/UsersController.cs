using bioSjenica.Models;
using bioSjenica.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Controllers {
  [Controller]
  [Route("/api/[controller]")]
  public class UsersController:ControllerBase {
    private readonly IUserRepository _userRepository;
    public UsersController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user) {
      return await _userRepository.Create(user);
    }
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers() {
      return await _userRepository.Get();
    }
    [HttpPatch]
    [Route("{SSN}")]
    public async Task<ActionResult<User>> UpdateUser([FromRoute]string SSN, [FromBody]User userPayload) {
      return await _userRepository.Update(userPayload, SSN);
    }
    [HttpDelete]
    [Route("{SSN}")]
    public async Task<ActionResult<User>> DeleteUser([FromRoute]string SSN) {
      return await _userRepository.Delete(SSN);
    }
  }
}