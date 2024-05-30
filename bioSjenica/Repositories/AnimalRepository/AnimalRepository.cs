using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using bioSjenica.Repositories.RegionRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories.AnimalRepository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly SqlContext _sqlContext;
        private readonly ILogger<AnimalRepository> _logger;
        private readonly IAnimalMapper _animalMapper;
        public AnimalRepository(SqlContext sqlContext, ILogger<AnimalRepository> logger, IAnimalMapper animalMapper)
        {
            _sqlContext = sqlContext;
            _logger = logger;
            _animalMapper = animalMapper;
        }

        public async Task<ReadAnimalDTO> Create(CreateAnimalDTO newAnimal)
        {   
            try
            {
                Animal animalToAdd = await _animalMapper.CreateToAnimal(newAnimal);
                //Add region to database and save
                _sqlContext.Add(animalToAdd);
                await _sqlContext.SaveChangesAsync();
                //Form a DTO for read
                return await _animalMapper.AnimalToRead(animalToAdd);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }

        public async Task<ReadAnimalDTO> Delete(string latinicOrCommonName)
        {
            try
            {
                var animalToDel = _sqlContext.Animals.FirstOrDefault(a => a.LatinicName == latinicOrCommonName || a.CommonName == latinicOrCommonName);
                if(animalToDel is null )
                {
                    _logger.LogError("Not found");
                    throw (RequestException)new NotFoundException("Animal");
                }
                _sqlContext.Remove(animalToDel);
                await _sqlContext.SaveChangesAsync();
                return await _animalMapper.AnimalToRead(animalToDel);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new NotFoundException("Animal");
            }
        }

        public async Task<List<ReadAnimalDTO>> Get()
        {
            int flagWith = 1;
            try
            {
                //Get info
                var animals = await _sqlContext.Animals
                    .Include(a => a.Regions)
                    .ToListAsync();

                //Map dtos
                List<ReadAnimalDTO> animalInfo = new();
                foreach(var animal in animals)
                {
                    animalInfo.Add(await _animalMapper.AnimalToRead(animal));
                }
                return animalInfo;
            }
            catch(Exception e)
            {
                //Handle database exceptions
                _logger.LogError(e, e.Message);
                throw new NotImplementedException();
            }
        }

        public async Task<ReadAnimalDTO> Update(string latinicOrCommonName, CreateAnimalDTO updateAnimalPayload)
        {
            try
            {
                //Get the required animal
                Animal animalToUpdate = await _sqlContext.Animals
                                        .Include(a => a.Regions)
                                        .FirstOrDefaultAsync(a => a.LatinicName.ToLower() == latinicOrCommonName.ToLower() || a.CommonName.ToLower() == latinicOrCommonName.ToLower());
                if(animalToUpdate is null)
                {
                    _logger.LogError("Animal not found");
                    throw (RequestException)new NotFoundException("Animal");
                }
                //Check for regions
                var updateAnimal = await _animalMapper.CreateToAnimal(updateAnimalPayload);
                //Update existing animal
                animalToUpdate.RingNumber = updateAnimal.RingNumber;
                animalToUpdate.LatinicName = updateAnimal.LatinicName;
                animalToUpdate.CommonName = updateAnimal.CommonName;
                //Remove children
                animalToUpdate.Regions = updateAnimal.Regions ?? animalToUpdate.Regions;
                animalToUpdate.FeedingGrounds = updateAnimal.FeedingGrounds ?? animalToUpdate.FeedingGrounds;
                //Save changes
                await _sqlContext.SaveChangesAsync();
                //Create readDTO from new animal
                return await _animalMapper.AnimalToRead(animalToUpdate);
            }
            catch(Exception e)
            {
                _logger.LogError("Internal problem");
                throw new NotImplementedException();
            }
        }
    }
}
