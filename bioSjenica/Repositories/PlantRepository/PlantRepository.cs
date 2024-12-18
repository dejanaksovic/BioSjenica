using System.Security.Cryptography.X509Certificates;
using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

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
        _sqlContext.Plants.Add(plantToAdd);
        
        try {
          await _sqlContext.SaveChangesAsync();
        }
        catch(Exception e) {
          throw new RequestException("Database error", errorCodes.INTERNAL_ERROR);
        }

        return _plantMapper.PlantToRead(plantToAdd);
      }
      public async Task<ReadPlantDTO> Delete(string latinicOrCommonName)
      {
          var plantToDelete = _sqlContext.Plants.FirstOrDefault(p => p.CommonName == latinicOrCommonName || p.LatinicName == latinicOrCommonName);
          if(plantToDelete is null) throw new RequestException("Plant not found", errorCodes.NOT_FOUND);
          
          _sqlContext.Plants.Remove(plantToDelete);
          try {
            await _sqlContext.SaveChangesAsync();
          }
          catch(Exception e) {
            throw new RequestException("Db error", errorCodes.INTERNAL_ERROR);
          }
          //Handle image removal
          // TODO: HANDLE RACE CONDITION
          Image.Delete(plantToDelete.ImageUrl);

          return _plantMapper.PlantToRead(plantToDelete);
      }
      public async Task<List<ReadPlantDTO>> Get(string? regionName)
      {
        List<Plant> plants;

        if(regionName is null) {
          plants = await _sqlContext.Plants.ToListAsync();
        }
        else {
          plants = await _sqlContext.Plants.Where(p => p.Regions.FirstOrDefault(r => r.Name == regionName) != null).ToListAsync();
        }

        return _plantMapper.PlantToReadList(plants);
      }
      public async Task<ReadPlantDTO> Update(string latinicOrCommonName, CreatePlantDTO plantPayload)
      {
          var plantToUpdate = await _sqlContext.Plants
                                    .Include(p => p.Regions)
                                    .FirstOrDefaultAsync(p => p.CommonName == latinicOrCommonName || p.LatinicName == latinicOrCommonName);
          if(plantToUpdate is null) throw new RequestException("Plant not found", errorCodes.NOT_FOUND);
          
          var newPlantInfo = await _plantMapper.CreateToPlant(plantPayload);
          //Check for image update 
          if(plantPayload.Image is not null && plantPayload.Image.Length > 0) {
            Image.Delete(plantToUpdate.ImageUrl);
            Image.Create("plants", plantPayload.CommonName, plantPayload.Image);
          }
          //Chck only for the name change
          if(plantToUpdate.ImageUrl != newPlantInfo.ImageUrl) {
            Image.Move(plantToUpdate.ImageUrl, newPlantInfo.ImageUrl);
          }
          plantToUpdate.CommonName = newPlantInfo.CommonName ?? plantToUpdate.CommonName;
          plantToUpdate.LatinicName = newPlantInfo.LatinicName ?? plantToUpdate.LatinicName;
          plantToUpdate.Regions = newPlantInfo.Regions ?? plantToUpdate.Regions;
          plantToUpdate.SpecialDecision = newPlantInfo.SpecialDecision ?? plantToUpdate.SpecialDecision;
          plantToUpdate.SpecialTime = newPlantInfo.SpecialTime ?? plantToUpdate.SpecialTime;
          plantToUpdate.ImageUrl = newPlantInfo.ImageUrl ?? plantToUpdate.ImageUrl;

          try {
            await _sqlContext.SaveChangesAsync();
          }
          catch(Exception e) {
            throw new RequestException("Error saving to db", errorCodes.INTERNAL_ERROR);
          }
          
          return _plantMapper.PlantToRead(plantToUpdate);
      }
    }
}