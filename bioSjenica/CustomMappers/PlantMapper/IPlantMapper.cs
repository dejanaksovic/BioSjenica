using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
  public interface IPlantMapper {
    public Task<Plant> CreateToPlant(CreatePlantDTO DTO);
    public ReadPlantDTO PlantToRead(Plant plant);
    public List<ReadPlantDTO> PlantToReadList(List<Plant> plants);
  }

}