using bioSjenica.Data;
using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.DTOs.Regions;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.CustomMappers
{
    public class RegionMapper : IRegionMapper
    {
        //Need databse :D
        private readonly SqlContext _sqlContext;
        private readonly ILogger<RegionMapper> _logger;
        private readonly IAnimalMapper _animalMapper;

        public RegionMapper(SqlContext sqlContext, ILogger<RegionMapper> logger, IAnimalMapper animalMapper)
        {
            _sqlContext = sqlContext;
            _logger = logger;
            _animalMapper = animalMapper;
        }
        public async Task<Region> CreateToRegion(CreateRegionDTO DTO)
        {
            //WARNING: A LOT OF REPETITION
            //Init setup for lists of items
            //Nullable cause it makes sence that u havea region that has no plants, nor animals, nor feeding grounds (just kill me)
            List<Animal> animals = null;
            List<Plant> plants = null;
            List<FeedingGround> feedingGrounds = null;
            //Check for animals
            if (!(DTO.AnimalsCommonOrLatinicNames is null))
            {
                animals = new List<Animal>();
                foreach (string name in DTO.AnimalsCommonOrLatinicNames)
                {
                    var animalToAdd = await _sqlContext.Animals.FirstOrDefaultAsync(a => a.CommonName == name || a.LatinicName == name);
                    if (animalToAdd is null)
                    {
                        _logger.LogError("Animal not found");
                        throw (RequestException)new NotFoundException("Animal");
                    }
                    animals.Add(animalToAdd);
                }
            }
            //Check for plants
            if (!(DTO.PlantsCommonOrLatinicNames is null))
            {
                plants = new List<Plant>();
                foreach (string name in DTO.PlantsCommonOrLatinicNames)
                {
                    var plantToAdd = await _sqlContext.Plants.FirstOrDefaultAsync(p => p.CommonName == name || p.LatinicName == name);
                    //Handle not found animal exception
                    if (plantToAdd is null)
                    {
                        _logger.LogError("Plant not found");
                        throw (RequestException)new NotFoundException("Plant");
                    }
                    plants.Add(plantToAdd);
                }
            }
            //Check for feeding grounds
            if (!(DTO.GroundNumbers is null))
            {
                feedingGrounds = new List<FeedingGround>();
                foreach (int number in DTO.GroundNumbers)
                {
                    var feedingGroundToAdd = await _sqlContext.FeedingGorunds.FirstOrDefaultAsync(fg => fg.GroundNumber == number);
                    //Handle not found animal exception
                    if (feedingGroundToAdd is null)
                    {
                        _logger.LogError("Feeding ground not found");
                        throw (RequestException)new NotFoundException("Feeding ground");
                    }
                    feedingGrounds.Add(feedingGroundToAdd);
                }
            }
            //Return mapping object
            return new Region()
            {
                Name = DTO.Name,
                Area = DTO.Area,
                Villages = DTO.Villages,
                ProtectionType = DTO.ProtectionType,
                Animals = animals,
                Plants = plants,
                FeedingGrounds = feedingGrounds,
            };
        }
        public ReadRegionDTO RegionToRead(Region region)
        {
            List<ReadAnimalDTO> animalDTOs = new List<ReadAnimalDTO>();

            if(!(region.Animals is null)) {
                foreach(var animal in region.Animals) {
                    animalDTOs.Add(_animalMapper.AnimalToRead(animal));
                } 
            }

            return new ReadRegionDTO
            {
                Name = region.Name,
                Area = region.Area,
                Villages = region.Villages,
                ProtectionType = region.ProtectionType,
                Plants = region.Plants,
                Animals = animalDTOs.Count() == 0 ? null : animalDTOs,
                FeedingGrounds = region.FeedingGrounds,
            };
        }

        public List<ReadRegionDTO> RegionToReadList (List<Region> regions) {
            List<ReadRegionDTO> toReturn = new();

            foreach(var region in regions) {
                toReturn.Add(this.RegionToRead(region));
            }

            return toReturn;
        }
    }
}
