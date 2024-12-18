using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers {
  public interface IAnimalMapper {
    public Task<Animal> CreateToAnimal(CreateAnimalDTO DTO);
    public ReadAnimalDTO AnimalToRead(Animal animal);
    public List<ReadAnimalDTO> AnimalToReadList(List<Animal> animals);

  }
}