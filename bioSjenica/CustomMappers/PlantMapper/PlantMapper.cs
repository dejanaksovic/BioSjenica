using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using bioSjenica.Utilities;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.CustomMappers {
  public class PlantMapper:IPlantMapper {
    private readonly SqlContext _sqlContex; 
    private readonly ILogger<PlantMapper> _logger; 
    public PlantMapper(SqlContext context, ILogger<PlantMapper> logger)
    {
      _sqlContex = context;
      _logger = logger;
    }
    public async Task<Plant> CreateToPlant(CreatePlantDTO DTO) {
      // Init setup
      List<Region>? regions = null;
      // Checking and adding regions
      if(!(DTO.RegionNames is null)) {
        regions = new List<Region>();
        foreach(var name in DTO.RegionNames) {
          var region = await _sqlContex.Regions.FirstOrDefaultAsync(r => r.Name == name);
          if(region is null) {
            _logger.LogError("Region not found");
            throw (RequestException)new NotFoundException("region");
          }
          regions.Add(region);
        }
      }
      // return animal with mappings
      return new Plant() {
        CommonName = DTO.CommonName,
        LatinicName = DTO.LatinicName,
        ImageUrl = "",
        SpecialDecision = DTO.SpecialDecision,
        SpecialTime = DTO.SpecialTime,
        Regions = regions,
      };
     }
    public async Task<ReadPlantDTO> PlantToRead(Plant plant) {
      return new ReadPlantDTO() {
        CommonName = plant.CommonName,
        LatinicName = plant.LatinicName,
        ImageUrl = plant.ImageUrl,
        Regions = plant.Regions,
        SpecialDecision = plant.SpecialDecision,
        SpecialTime = plant.SpecialTime,
      };
    }

  }
}