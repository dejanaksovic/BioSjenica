using System.Reflection;
using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using bioSjenica.Utilities;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories.AnimalRepository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly SqlContext _sqlContext;
        private readonly ILogger<AnimalRepository> _logger;
        private readonly IAnimalMapper  _animalMapper;
        public AnimalRepository(SqlContext sqlContext, ILogger<AnimalRepository> logger, IAnimalMapper animalMapper)
        {
            _sqlContext = sqlContext;
            _logger = logger;
            _animalMapper = animalMapper;
        }

        // Validation
        // TODO: IMPLEMENT

        public async Task<ReadAnimalDTO> Create(CreateAnimalDTO newAnimal)
        {   
            Animal animalToCreate = await _animalMapper.CreateToAnimal(newAnimal);
            _sqlContext.Add(animalToCreate);
            
            // Image creation
            try {
                string url = await Image.Create("animals", animalToCreate.CommonName, newAnimal.Image);
                animalToCreate.ImageUrl = url;
            }
            catch(Exception e) {
                _logger.LogError("Image saving failed", e);
                throw new RequestException("Failed saving the image", errorCodes.INTERNAL_ERROR);
            }

            try {
                await _sqlContext.SaveChangesAsync();
            }
            catch(Exception e) {
                throw new RequestException("Internal error", errorCodes.INTERNAL_ERROR);
            }

            return _animalMapper.AnimalToRead(animalToCreate);
        }

        public async Task<ReadAnimalDTO> Delete(string latinicOrCommonName)
        {
            var animalToDel = _sqlContext.Animals.FirstOrDefault(a => a.LatinicName == latinicOrCommonName || a.CommonName == latinicOrCommonName);
            if(animalToDel is null) throw new RequestException("Animal not found", errorCodes.NOT_FOUND, ErrorDict.CreateDict("LatinicOrCommonName", latinicOrCommonName));

            _sqlContext.Animals.Remove(animalToDel);
            try {
                await _sqlContext.SaveChangesAsync();
            }
            catch(Exception e) {
                throw new RequestException("Database error", errorCodes.INTERNAL_ERROR);
            }

            return _animalMapper.AnimalToRead(animalToDel);
        }

        public async Task<List<ReadAnimalDTO>> Get(string? regionName)
        {
            List<Animal>? animals;

            try {
                if(regionName is null) {
                    animals = await _sqlContext.Animals.ToListAsync();
                }
                else {
                    animals = await _sqlContext.Animals.Where( a => a.Regions.FirstOrDefault(r => r.Name == regionName) != null).ToListAsync();
                }
            }
            catch(Exception e) {
                _logger.LogError("Db failed");
                throw new RequestException("Db error", errorCodes.INTERNAL_ERROR);
            }

            return _animalMapper.AnimalToReadList(animals);
        }

        public async Task<ReadAnimalDTO> Update(string latinicOrCommonName, CreateAnimalDTO updateAnimalPayload)
        {
            var animal = _sqlContext.Animals.FirstOrDefault(a => a.LatinicName == latinicOrCommonName || a.CommonName == latinicOrCommonName);
            if(animal is null) throw new RequestException("Not found animal", errorCodes.NOT_FOUND, ErrorDict.CreateDict("LatinicOrCommonName", latinicOrCommonName));

            if(updateAnimalPayload.RingNumber is not null) animal.RingNumber = updateAnimalPayload.RingNumber;
            if(updateAnimalPayload.LatinicName is not null) animal.LatinicName = updateAnimalPayload.LatinicName;
            if(updateAnimalPayload.CommonName is not null) animal.CommonName = updateAnimalPayload.CommonName;

            try {
                await _sqlContext.SaveChangesAsync();
            }
            catch(Exception e) {
                throw new RequestException("Database error", errorCodes.INTERNAL_ERROR);
            }

            return _animalMapper.AnimalToRead(animal);
        }
    }
}
