using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
  public interface IPlantMapper {
    public Task<Plant> CreateToPlant(CreatePlantDTO DTO);
    public Task<ReadPlantDTO> PlantToRead(Plant plant);
  }

}