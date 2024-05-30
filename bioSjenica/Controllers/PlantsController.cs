using bioSjenica.CustomMappers;
using bioSjenica.DTOs;
using bioSjenica.Repositories;
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
    public async Task<ActionResult<ReadPlantDTO>> CreatePlant([FromBody] CreatePlantDTO plantPayload) {
      return Ok(await _plantRepository.Create(plantPayload));
    }
    [HttpGet]
    public async Task<ActionResult<List<ReadPlantDTO>>> GetPlants() {
      return Ok(await _plantRepository.Get());
    }
    [HttpPatch]
    [Route("{latinicOrCommonName}")]
    public async Task<ActionResult<ReadPlantDTO>> Update([FromRoute] string latinicOrCommonName, [FromBody]CreatePlantDTO plantPayload) {
      return Ok(await _plantRepository.Update(latinicOrCommonName, plantPayload));
    }
    [HttpDelete]
    [Route("{latinicOrCommonName}")]
    public async Task<ActionResult<ReadPlantDTO>> Delete([FromRoute] string latinicOrCommonName) {
      return Ok(await _plantRepository.Delete(latinicOrCommonName));
    }
  }
}