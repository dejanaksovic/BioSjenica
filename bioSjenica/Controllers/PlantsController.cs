using bioSjenica.DTOs;
using bioSjenica.Repositories;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Controllers {
  [Route("api/[controller]")]
  [Controller]
  public class PlantsController:ControllerBase {
    private readonly IPlantRepository _plantRepository;
    private readonly ILogger<PlantsController> _logger;
    public PlantsController(IPlantRepository plantRepository, ILogger<PlantsController> logger)
    {
      _plantRepository = plantRepository;
      _logger = logger;
    }

    [HttpPost]
    // [Authorize(Roles = $"{Roles.Worker}")]
    public async Task<ActionResult<ReadPlantDTO>> CreatePlant([FromForm] CreatePlantDTO plantPayload) {
      return Ok(await _plantRepository.Create(plantPayload));
    }

    [HttpGet]
    public async Task<ActionResult<List<ReadPlantDTO>>> GetPlants([FromQuery] string regionName) {
      return Ok(await _plantRepository.Get(regionName));
    }
    
    [HttpPatch]
    // [Authorize(Roles = $"{Roles.Worker}")]
    [Route("{latinicOrCommonName}")]
    public async Task<ActionResult<ReadPlantDTO>> Update([FromRoute] string latinicOrCommonName, [FromForm]CreatePlantDTO plantPayload) {
      return Ok(await _plantRepository.Update(latinicOrCommonName, plantPayload));
    }
    [HttpDelete]
    // [Authorize(Roles = $"{Roles.Worker}")]
    [Route("{latinicOrCommonName}")]
    public async Task<ActionResult<ReadPlantDTO>> Delete([FromRoute] string latinicOrCommonName) {
      return Ok(await _plantRepository.Delete(latinicOrCommonName));
    }
  }
}