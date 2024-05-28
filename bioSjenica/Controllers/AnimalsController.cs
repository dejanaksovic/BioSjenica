using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Repositories.AnimalRepository;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<CreateAnimalDTO>> Create([FromBody]CreateAnimalDTO animal)
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
        public async Task<ActionResult<List<CreateAnimalDTO>>> Read()
        {
            try
            {
                var animals = await _animalRepository.Get();
                return Ok(animals);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPatch]
        [Route("{latinicOrCommonName}")]
        public async Task<ActionResult<CreateAnimalDTO>> Update([FromRoute] string latinicOrCommonName, [FromBody] CreateAnimalDTO updateRequest)
        {
            try
            {
                var updatedAnimal = await _animalRepository.Update(latinicOrCommonName, updateRequest);
                return Ok(updatedAnimal);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
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
