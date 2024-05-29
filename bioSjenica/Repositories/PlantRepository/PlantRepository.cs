using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories {
    public class PlantRepository : IPlantRepository
    {
      private readonly SqlContext _sqlContext;
      private readonly ILogger<PlantRepository> _logger;
      private readonly IPlantMapper _plantMapper;
      public PlantRepository(SqlContext context, ILogger<PlantRepository> logger, IPlantMapper plantMapper)
      {
        _sqlContext = context;
        _logger = logger;
        _plantMapper = plantMapper;
      }
      public async Task<ReadPlantDTO> Create(CreatePlantDTO plantPayload)
      {
        var plantToAdd = await _plantMapper.CreateToPlant(plantPayload);
        try {
          _sqlContext.Plants.Add(plantToAdd);
          await _sqlContext.SaveChangesAsync();
          return await _plantMapper.PlantToRead(plantToAdd);
        }
        catch(Exception e) {
          // TODO: Implement server error;
          _logger.LogError("Internal server error");
          throw new NotImplementedException();
        }
      }
      public async Task<ReadPlantDTO> Delete(string latinicOrCommonName)
      {
          var plantToDelete = _sqlContext.Plants.FirstOrDefault(p => p.CommonName == latinicOrCommonName || p.LatinicName == latinicOrCommonName);
          if(plantToDelete is null) {
            // TODO: IMplement plant not found
            _logger.LogError("Plant not found");
            throw new NotImplementedException();
          }
          _sqlContext.Plants.Remove(plantToDelete);
          await _sqlContext.SaveChangesAsync();
          return await _plantMapper.PlantToRead(plantToDelete);
      }
      public async Task<List<ReadPlantDTO>> Get()
      {
          List<ReadPlantDTO> plantDtosToReturn = new List<ReadPlantDTO>();
          var plantsToReturn = await _sqlContext.Plants.ToListAsync();
          foreach(var plant in plantsToReturn) {
            plantDtosToReturn.Add(await _plantMapper.PlantToRead(plant));
          }
          return plantDtosToReturn;
      }
      public async Task<ReadPlantDTO> Update(string latinicOrCommonName, CreatePlantDTO plantPayload)
      {
          var plantToUpdate = await _sqlContext.Plants
                                    .Include(p => p.Regions)
                                    .FirstOrDefaultAsync(p => p.CommonName == latinicOrCommonName || p.LatinicName == latinicOrCommonName);
          var newPlantInfo = await _plantMapper.CreateToPlant(plantPayload);
          plantToUpdate.CommonName = newPlantInfo.CommonName ?? plantToUpdate.CommonName;
          plantToUpdate.LatinicName = newPlantInfo.LatinicName ?? plantToUpdate.LatinicName;
          plantToUpdate.Regions = newPlantInfo.Regions ?? plantToUpdate.Regions;
          plantToUpdate.SpecialDecision = newPlantInfo.SpecialDecision ?? plantToUpdate.SpecialDecision;
          plantToUpdate.SpecialTime = newPlantInfo.SpecialTime ?? plantToUpdate.SpecialTime;
          plantToUpdate.ImageUrl = newPlantInfo.ImageUrl ?? plantToUpdate.ImageUrl;

          await _sqlContext.SaveChangesAsync();
          return await _plantMapper.PlantToRead(plantToUpdate);
      }
    }
}