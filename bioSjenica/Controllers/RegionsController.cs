using bioSjenica.Models;
using bioSjenica.Payloads;
using bioSjenica.Repositories.RegionRepository;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<Region>> CreateRegion([FromBody] RegionDTO payload)
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
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Region>> UpdateRegion([FromBody] RegionDTO payload, [FromRoute] int id)
        {
            var updatedRegion = await _regionRepository.UpdateRegion(payload, id);
            if (updatedRegion is null)
                return NotFound("The required region is not found");
            return updatedRegion;
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Region>> DeleteRegion([FromRoute] int id)
        {
            var deletedRegion = await _regionRepository.DeleteRegionById(id);
            if (deletedRegion is null)
                return NotFound("The region was not found");
            return Ok(deletedRegion);
        }
    }
}
