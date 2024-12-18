using bioSjenica.DTOs;
using bioSjenica.Repositories;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Controllers {
  [Controller]
  [Route("/api/[controller]")]
  public class FeedingGroundsController:ControllerBase {
    private readonly IFeedingGroundRepository _feedingGroundsRepository;
    public FeedingGroundsController(IFeedingGroundRepository feedingGroundRepository)
    {
      _feedingGroundsRepository = feedingGroundRepository;
    }

    [HttpPost]
    // [Authorize(Roles = $"{Roles.Worker}")]
    public async Task<ActionResult<ReadFeedingGroundDTO>> CreateFeedingGround([FromBody] CreateFeedingGroundDTO feedingGroundsPayload) {
      return Ok(await _feedingGroundsRepository.Create(feedingGroundsPayload));
    }

    [HttpGet]
    public async Task<ActionResult<ReadFeedingGroundDTO>> GetFeedingGrounds([FromQuery] int month) {
      return Ok(await _feedingGroundsRepository.Get(month));
    }

    [HttpPatch]
    // [Authorize(Roles = $"{Roles.Worker}")]
    [Route("{groundsNumber}")]
    public async Task<ActionResult<ReadFeedingGroundDTO>> UpdateFeeingGround([FromBody] CreateFeedingGroundDTO feedingGroundPayload, [FromRoute] int groundsNumber) {
      return Ok(await _feedingGroundsRepository.Update(feedingGroundPayload, groundsNumber));
    }

    [HttpDelete]
    [Authorize(Roles = $"{Roles.Worker}")]
    // [Route("{groundsNumber}")]
    public async Task<ActionResult<ReadFeedingGroundDTO>> DeleteFeedingGround([FromRoute] int groundsNumber) {
      return Ok(await _feedingGroundsRepository.Delete(groundsNumber));
    }
  }
}