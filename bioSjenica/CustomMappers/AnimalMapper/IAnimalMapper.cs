using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
  public interface IAnimalMapper {
    public Task<Animal> CreateToAnimal(CreateAnimalDTO DTO);
    public Task<ReadAnimalDTO> AnimalToRead(Animal animal);
  }
}