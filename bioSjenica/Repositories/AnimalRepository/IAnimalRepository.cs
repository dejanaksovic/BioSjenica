using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Models;

namespace bioSjenica.Repositories.AnimalRepository
{
    public interface IAnimalRepository
    {
        public Task<ReadAnimalDTO> Create(CreateAnimalDTO newAnimal);
        public Task<List<ReadAnimalDTO>> Get();
        public Task<ReadAnimalDTO> Update(string latinicOrCommonName, CreateAnimalDTO updateAnimal);
        public Task<ReadAnimalDTO> Delete(string latinicOrCommonName); 
    }
}
