using bioSjenica.DTOs;
using bioSjenica.Repositories;
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
    public async Task<ActionResult<ReadFeedingGroundDTO>> CreateFeedingGround([FromBody] CreateFeedingGroundDTO feedingGroundsPayload) {
      return Ok(await _feedingGroundsRepository.Create(feedingGroundsPayload));
    }
    [HttpGet]
    public async Task<ActionResult<ReadFeedingGroundDTO>> GetFeedingGrounds([FromQuery] int month) {
      return Ok(await _feedingGroundsRepository.Get(month));
    }
    [HttpPatch]
    [Route("{groundsNumber}")]
    public async Task<ActionResult<ReadFeedingGroundDTO>> UpdateFeeingGround([FromBody] CreateFeedingGroundDTO feedingGroundPayload, [FromRoute] int groundsNumber) {
      return Ok(await _feedingGroundsRepository.Update(feedingGroundPayload, groundsNumber));
    }
    [HttpDelete]
    [Route("{groundsNumber}")]
    public async Task<ActionResult<ReadFeedingGroundDTO>> DeleteFeedingGround([FromRoute] int groundsNumber) {
      return Ok(await _feedingGroundsRepository.Delete(groundsNumber));
    }
  }
}