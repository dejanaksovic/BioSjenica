using bioSjenica.Controllers;
using bioSjenica.Data;
using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Models;
using bioSjenica.Repositories.RegionRepository;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories.AnimalRepository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly SqlContext _sqlContext;
        private readonly ILogger<AnimalRepository> _logger;
        private readonly IRegionRepository _regionRepository;
        public AnimalRepository(SqlContext sqlContext, ILogger<AnimalRepository> logger, IRegionRepository regionRepository)
        {
            _sqlContext = sqlContext;
            _regionRepository = regionRepository;
            _logger = logger;
        }

        public async Task<ReadAnimalDTO> Create(CreateAnimalDTO newAnimal)
        {   
            try
            {
                //Check if there are regions added
                List<Region> regions = new List<Region>();
                if (!(newAnimal.RegionNames is null))
                {
                    foreach (string name in newAnimal.RegionNames)
                    {
                        Region requestedRegion = await _sqlContext.Regions.FirstOrDefaultAsync(r => r.Name == name);
                        // Handle not found region exception
                        if (requestedRegion is null)
                        {
                            _logger.LogError("Requested region not found");
                            throw new NotImplementedException();
                        }
                        //Add requested region to the list
                        regions.Add(requestedRegion);
                    }
                }

                //Check for feeding grounds
                List<FeedingGround> feedingGrounds = new List<FeedingGround>();
                if(!(newAnimal.GroundNumbers is null))
                {
                    foreach(var groundNumber in newAnimal.GroundNumbers)
                    {
                        var feedingGround = await _sqlContext.FeedingGorunds.FirstOrDefaultAsync(fg => fg.GroundNumber == groundNumber);
                        //TODO: Handle not found exception
                        if (feedingGround is null)
                            throw new NotImplementedException();
                        feedingGrounds.Add(feedingGround);
                    }
                }

                //Create new animal
                _logger.LogInformation($"{regions.Count()}");
                Animal createdAnimal = new()
                {
                     LatinicName = newAnimal.LatinicName,
                     CommonName = newAnimal.CommonName,
                     RingNumber = newAnimal.RingNumber,
                     ImageUrl = newAnimal.ImageUrl,
                     Regions = regions.Count() != 0 ? regions : null,
                     FeedingGrounds = feedingGrounds.Count() != 0 ? feedingGrounds : null,
                };

                //Add region to database and save
                _sqlContext.Add(createdAnimal);
                await _sqlContext.SaveChangesAsync();
                //Form a DTO for read
                ReadAnimalDTO animalToReturn = new()
                {
                    LatinicName = createdAnimal.LatinicName,
                    CommonName = createdAnimal.CommonName,
                    RingNumber = createdAnimal.RingNumber,
                    ImageUrl = createdAnimal.ImageUrl,
                };
                return animalToReturn;
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
                    //TODO: Handle not found animal
                    _logger.LogError("Not found");
                    throw new NotImplementedException();
                }
                _sqlContext.Remove(animalToDel);
                await _sqlContext.SaveChangesAsync();
                return new ReadAnimalDTO()
                {
                    RingNumber = animalToDel.RingNumber,
                    LatinicName = animalToDel.LatinicName,
                    CommonName = animalToDel.CommonName,
                    ImageUrl = animalToDel.CommonName,
                    Regions = animalToDel.Regions,
                    FeedingGrounds = animalToDel.FeedingGrounds,
                };
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new NotImplementedException();
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
                    animalInfo.Add(new ReadAnimalDTO() {
                        CommonName = animal.CommonName,
                        LatinicName = animal.LatinicName,
                        RingNumber = animal.RingNumber,
                        ImageUrl = animal.ImageUrl,
                        Regions = flagWith == 0 ? null : animal.Regions,
                        FeedingGrounds = flagWith == 0 ? null : animal.FeedingGrounds
                    });
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

        public async Task<ReadAnimalDTO> Update(string latinicOrCommonName, CreateAnimalDTO updateAnimal)
        {
            try
            {
                //Get the required animal
                Animal animalToUpdate = await _sqlContext.Animals.FirstOrDefaultAsync(a => a.LatinicName.ToLower() == latinicOrCommonName.ToLower() || a.CommonName.ToLower() == latinicOrCommonName.ToLower());
                //Handle not found animal exception
                if(animalToUpdate is null)
                {
                    throw new NotImplementedException();
                }
                //Set new regions
                List<Region> newRegions = new();
                //Check for reagion
                if(!(updateAnimal.RegionNames is null))
                {
                    foreach(var regionName in updateAnimal.RegionNames)
                    {
                        var region = await _sqlContext.Regions.FirstOrDefaultAsync(r => r.Name == regionName);
                        //TODO: Handle not found region
                        if (region is null)
                        {
                            _logger.LogError("Region not found");
                            throw new NotImplementedException();
                        }
                        newRegions.Add(region);
                    }
                }
                //Set new regions
                List<FeedingGround> newFeedingGrounds = new();
                //Check for reagion
                if (!(updateAnimal.GroundNumbers is null))
                {
                    foreach (var number in updateAnimal.GroundNumbers)
                    {
                        var feedingGround = await _sqlContext.FeedingGorunds.FirstOrDefaultAsync(fg => fg.GroundNumber == number);
                        //TODO: Handle not found feeding ground
                        if (feedingGround is null)
                        {
                            _logger.LogError("Feeding ground not found");
                            throw new NotImplementedException();
                        }
                        newFeedingGrounds.Add(feedingGround);
                    }
                }
                //Update existing animal
                animalToUpdate.RingNumber = updateAnimal.RingNumber;
                animalToUpdate.LatinicName = updateAnimal.LatinicName;
                animalToUpdate.CommonName = updateAnimal.CommonName;
                animalToUpdate.Regions = newRegions.Count() == 0 ? animalToUpdate.Regions : newRegions;
                animalToUpdate.FeedingGrounds = newFeedingGrounds.Count() == 0 ? animalToUpdate.FeedingGrounds : newFeedingGrounds;

                //Save changes
                await _sqlContext.SaveChangesAsync();
                //Create readDTO from new animal
                ReadAnimalDTO animalToReturn = new()
                {
                    RingNumber = animalToUpdate.RingNumber,
                    LatinicName = animalToUpdate.LatinicName,
                    CommonName = animalToUpdate.CommonName,
                    Regions = animalToUpdate.Regions,
                    FeedingGrounds = animalToUpdate.FeedingGrounds
                };
                return animalToReturn;
            }
            catch(Exception e)
            {
                _logger.LogError("Internal problem");
                throw new NotImplementedException();
            }
        }
    }
}
