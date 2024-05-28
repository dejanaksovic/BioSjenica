using bioSjenica.Data;
using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.DTOs.Regions;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.CustomMappers {
    public class AnimalMapper : IAnimalMapper
    {
      private readonly ILogger<AnimalMapper> _logger;
      private readonly SqlContext _sqlContext;
      public AnimalMapper(ILogger<AnimalMapper> logger, SqlContext context)
      {
        _logger = logger;
        _sqlContext = context;
      }
      public async Task<ReadAnimalDTO> AnimalToRead(Animal animal)
      {
        // warning: not adding more nesting, it kinda stops at level one
        // If you want to get the information about individual regions make seperate api call
        List<ReadRegionDTO> regionsDTO = new List<ReadRegionDTO>();
        if(!(animal.Regions is null)) {
          foreach(Region region in animal.Regions) {
            var dto = new ReadRegionDTO(){
              Name = region.Name,
              Area = region.Area,
              Villages = region.Villages,
              ProtectionType = region.ProtectionType,
            };
            regionsDTO.Add(dto);
          }
        }
        return new ReadAnimalDTO() {
          RingNumber = animal.RingNumber,
          CommonName = animal.CommonName,
          LatinicName = animal.LatinicName,
          ImageUrl = animal.ImageUrl,
          Regions = regionsDTO.Count() == 0 ? null : regionsDTO,
          FeedingGrounds = animal.FeedingGrounds,
        };
      }

      public async Task<Animal> CreateToAnimal(CreateAnimalDTO DTO)
      {
        //Init setup
        List<Region> regions = null;
        List<FeedingGround> feedingGrounds = null;
        //Check for regions
        if(!(DTO.RegionNames is null)) {
          regions = new List<Region>();
          foreach(var regionName in DTO.RegionNames) {
            var region = await _sqlContext.Regions.FirstOrDefaultAsync(r => r.Name == regionName);
            //TODO: Handle not found region
            if(region is null) {
              _logger.LogError("Region not found");
              throw new NotImplementedException();
            }
            regions.Add(region);
          }
        }
        //Check for feeding grounds
        if(!(DTO.GroundNumbers is null)) {
          feedingGrounds = new List<FeedingGround>();
          foreach(var number in DTO.GroundNumbers) {
            var feedingGround = await _sqlContext.FeedingGorunds.FirstOrDefaultAsync(fg => fg.GroundNumber == number);
            //TODO: Handle not found feeding ground
            if(feedingGround is null) {
              _logger.LogError("Feeding ground not found");
              throw new NotImplementedException();
            }
            feedingGrounds.Add(feedingGround);
          }
        }
        //Return values
        return new Animal(){
          LatinicName = DTO.LatinicName,
          CommonName = DTO.CommonName,
          RingNumber = DTO.RingNumber,
          ImageUrl = DTO.ImageUrl,
          Regions = regions,
          FeedingGrounds = feedingGrounds,
        };
      }
    }
}