using bioSjenica.DTOs;
using bioSjenica.Models;
using bioSjenica.Repositories;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Controllers {
  [Controller]
  [Authorize(Roles = $"{Roles.Admin}")]
  [Route("/api/[controller]")]
  public class UsersController:ControllerBase {
    private readonly IUserRepository _userRepository;
    public UsersController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }
    [HttpPost]
    public async Task<ActionResult<ReadUserDTO>> CreateUser([FromBody] User user) {
      return await _userRepository.Create(user);
    }
    [HttpGet]
    public async Task<ActionResult<List<ReadUserDTO>>> GetUsers() {
      return await _userRepository.Get();
    }
    [HttpPatch]
    [Route("{SSN}")]
    public async Task<ActionResult<ReadUserDTO>> UpdateUser([FromRoute]string SSN, [FromBody]User userPayload) {
      return await _userRepository.Update(userPayload, SSN);
    }
    [HttpDelete]
    [Route("{SSN}")]
    public async Task<ActionResult<ReadUserDTO>> DeleteUser([FromRoute]string SSN) {
      return await _userRepository.Delete(SSN);
    }
  }
}