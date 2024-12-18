using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Exceptions;
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
      //Check for regions
      if(!(DTO.RegionName is null)) {
        region = await _sqlContext.Regions.FirstOrDefaultAsync(r => r.Name == DTO.RegionName);
        if(region is null) {
          _logger.LogError("Region not found");
          throw (RequestException)new NotFoundException("Region");
        }
      }
      //Check for animals
      if(!(DTO.AnimalsLatinicOrCommonName is null)) {
        foreach(var name in DTO.AnimalsLatinicOrCommonName) {
          var animal = await _sqlContext.Animals.FirstOrDefaultAsync(a => a.LatinicName == name || a.CommonName == name);
          if(animal is null) {
            _logger.LogError("Animal not found");
            throw (RequestException)new NotFoundException("Animal");
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
        StartWork = (int)DTO.StartWork,
        EndWork = (int)DTO.EndWork,
        RegionId = region.Id,
      };
    }
    
    public ReadFeedingGroundDTO FeedingGroundToRead(FeedingGround feedingGround)
    {
        return new ReadFeedingGroundDTO() {
          GroundNumber = feedingGround.GroundNumber,
          Region = feedingGround.Region,
          StartWork = feedingGround.StartWork,
          EndWork = feedingGround.EndWork,
          Animals = feedingGround.Animals,
        };
    }

    public List<ReadFeedingGroundDTO> FeedingToReadList(List<FeedingGround> feedingGrounds) {
      List<ReadFeedingGroundDTO> toReturn = new();

      foreach(var fg in feedingGrounds) {
        toReturn.Add(this.FeedingGroundToRead(fg));
      }

      return toReturn;
    }
  }
}