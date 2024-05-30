using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.CustomMappers {
  public class FeedingGroundMapper : IFeedingGroundsMapper {
    private readonly SqlContext _sqlContext;
    private readonly ILogger<FeedingGroundMapper> _logger;
    public FeedingGroundMapper(SqlContext context, ILogger<FeedingGroundMapper> logger)
    {
      _sqlContext = context;
      _logger = logger;
    }

    public async Task<FeedingGround> CreateToFeedingGround(CreateFeedingGroundDTO DTO)
    {
      Region region = null;
      List<Animal> animals = new List<Animal>();
      //Check for reagion
      if(!(DTO.RegionName is null)) {
        region = await _sqlContext.Regions.FirstOrDefaultAsync(r => r.Name == DTO.RegionName);
        if(region is null) {
          // Handle not found region
          _logger.LogError("Region not found");
          throw new NotImplementedException();
        }
      }
      //Check for animals
      if(!(DTO.AnimalsLatinicOrCommonName is null)) {
        foreach(var name in DTO.AnimalsLatinicOrCommonName) {
          var animal = await _sqlContext.Animals.FirstOrDefaultAsync(a => a.LatinicName == name || a.CommonName == name);
          if(animal is null) {
            //Handle not found animal
            _logger.LogError("Animal not found");
            throw new NotImplementedException();
          }
          animals.Add(animal);
        }
      }
      //Return a feedingGround
      return new FeedingGround() {
        // TODO: HANDLE DEFAULT VALUES
        GroundNumber = DTO.GroundNumber ?? 0,
        Region = region,
        Animals = animals,
        StartWork = DTO.StartWork ?? new DateTime(),
        EndWork = DTO.EndWork ?? new DateTime(),
        RegionId = region.Id,
      };
    }
    
    public async Task<ReadFeedingGroundDTO> FeedingGroundToRead(FeedingGround feedingGround)
    {
        return new ReadFeedingGroundDTO() {
          GroundNumber = feedingGround.GroundNumber,
          Region = feedingGround.Region,
          StartWork = feedingGround.StartWork,
          EndWork = feedingGround.EndWork,
          Animals = feedingGround.Animals,
        };
    }
    }
}