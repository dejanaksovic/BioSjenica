using bioSjenica.DTOs;
using bioSjenica.Models;

namespace bioSjenica.Repositories {
  public interface IPlantRepository {
    public Task<ReadPlantDTO> Create(CreatePlantDTO plantPayload);
    public Task<List<ReadPlantDTO>> Get(string? regionFilter);
    public Task<ReadPlantDTO> Update(string latinicOrCommonName, CreatePlantDTO plantPayload);
    public Task<ReadPlantDTO> Delete(string latinicOrCommonName);
  }
}