using bioSjenica.DTOs.Regions;
using bioSjenica.Models;
using bioSjenica.Repositories.RegionRepository;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;

        public RegionsController(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Region>> CreateRegion([FromBody] CreateRegionDTO payload)
        {
            var createdRegion = await _regionRepository.CreateRegion(payload);
            if (createdRegion is null)
                return BadRequest(404);
            return Ok(createdRegion);
        }
        [HttpGet]
        public async Task<ActionResult<List<Region>>> GetAllRegions()
        {
            var regions = await _regionRepository.GetAllRegions();
            return Ok(regions);
        }
        [HttpPatch]
        [Route("{regionName}")]
        public async Task<ActionResult<ReadRegionDTO>> UpdateRegion([FromBody] CreateRegionDTO payload, [FromRoute] string regionName)
        {
            var updatedRegion = await _regionRepository.UpdateRegion(payload, regionName);
            if (updatedRegion is null)
                return NotFound("The required region is not found");
            return Ok(updatedRegion);
        }
        [HttpDelete]
        [Route("{regionName}")]
        public async Task<ActionResult<Region>> DeleteRegion([FromRoute] string regionName)
        {
            var deletedRegion = await _regionRepository.DeleteRegionByName(regionName);
            if (deletedRegion is null)
                return NotFound("The region was not found");
            return Ok(deletedRegion);
        }
    }
}
