using System.Security.Cryptography.X509Certificates;
using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
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
          //Handle image saving
          if(!(plantPayload.Image is null) && plantPayload.Image.Length > 0) {
            var res = await Image.Create("plants", plantPayload.CommonName, plantPayload.Image);
          plantToAdd.ImageUrl = res.StartsWith("success") ? res.Split("//")[1] : "no-image.jpg";
          }
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
            _logger.LogError("Plant not found");
            throw (RequestException)new NotFoundException("Plant");
          }
          //Handle image removal
          Image.Delete(plantToDelete.ImageUrl);
          _sqlContext.Plants.Remove(plantToDelete);
          await _sqlContext.SaveChangesAsync();
          return await _plantMapper.PlantToRead(plantToDelete);
      }
      public async Task<List<ReadPlantDTO>> Get(string? regionName)
      {
          List<ReadPlantDTO> plantDtosToReturn = new List<ReadPlantDTO>();
          var plantsToReturn = await _sqlContext.Plants
                               .Include(p => p.Regions)
                               .ToListAsync();
          _logger.LogInformation($"We should be in with region name: {regionName}");
          if(!(regionName is null)) {
            plantsToReturn = plantsToReturn.Where(p => p.Regions.FirstOrDefault(r => r.Name == regionName) != null).ToList();
            //Handle not found plants on region
            throw new NotFoundException("Region");
          }
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
          if(plantToUpdate is null) {
            throw (RequestException)new NotFoundException("Plant");
          }
          
          var newPlantInfo = await _plantMapper.CreateToPlant(plantPayload);
          //Check for image update 
          if(!(plantPayload.Image is null) && plantPayload.Image.Length > 0) {
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

          await _sqlContext.SaveChangesAsync();
          return await _plantMapper.PlantToRead(plantToUpdate);
      }
    }
}