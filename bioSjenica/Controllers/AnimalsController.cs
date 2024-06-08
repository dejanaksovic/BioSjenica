using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Repositories.AnimalRepository;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalRepository _animalRepository;
        public AnimalsController(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }
        [HttpPost]
        [Authorize(Roles = $"{Roles.Worker}")]
        public async Task<ActionResult<CreateAnimalDTO>> Create([FromForm]CreateAnimalDTO animal)
        {
            try
            {
                var animalToReturn = await _animalRepository.Create(animal);
                return Ok(animalToReturn);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<CreateAnimalDTO>>> Get([FromQuery]string? regionName)
        {
            try
            {
                var animals = await _animalRepository.Get(regionName);
                return Ok(animals);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPatch]
        [Authorize(Roles = $"{Roles.Worker}")]
        [Route("{latinicOrCommonName}")]
        public async Task<ActionResult<CreateAnimalDTO>> Update([FromRoute] string latinicOrCommonName, [FromBody] CreateAnimalDTO updateRequest)
        {
            var updatedAnimal = await _animalRepository.Update(latinicOrCommonName, updateRequest);
            return Ok(updatedAnimal);
        }
        [HttpDelete]
        [Authorize(Roles = $"{Roles.Worker}")]
        [Route("{latinicOrCommonName}")]
        public async Task<ActionResult<CreateAnimalDTO>> Delete([FromRoute] string latinicOrCommonName)
        {
            try
            {
                var deletedAnimal = await _animalRepository.Delete(latinicOrCommonName);
                return Ok(deletedAnimal);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
