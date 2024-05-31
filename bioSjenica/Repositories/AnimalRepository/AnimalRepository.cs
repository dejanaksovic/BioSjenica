using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs.AmnimalsDTO;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using bioSjenica.Repositories.RegionRepository;
using bioSjenica.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories.AnimalRepository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly SqlContext _sqlContext;
        private readonly ILogger<AnimalRepository> _logger;
        private readonly IAnimalMapper _animalMapper;
        public AnimalRepository(SqlContext sqlContext, ILogger<AnimalRepository> logger, IAnimalMapper animalMapper)
        {
            _sqlContext = sqlContext;
            _logger = logger;
            _animalMapper = animalMapper;
        }

        public async Task<ReadAnimalDTO> Create(CreateAnimalDTO newAnimal)
        {   
            try
            {
                Animal animalToAdd = await _animalMapper.CreateToAnimal(newAnimal);
                //Saving the image
                if(!(newAnimal.Image is null) && newAnimal.Image.Length > 0) {
                    if(!newAnimal.Image.ContentType.StartsWith("image")) {
                        throw new NotFoundException("image");
                    }
                    //Image extention
                    var imageContentType = newAnimal.Image.ContentType.Split("/")[1];
                    var path = BasePath.Get();
                    _logger.LogInformation($"Saving to ${path}");
                    using (FileStream fileStream = File.Create($"{path}/images/{newAnimal.CommonName}.{imageContentType}")) {
                        await newAnimal.Image.CopyToAsync(fileStream);
                    }
                }

                //Add region to database and save
                _sqlContext.Add(animalToAdd);
                await _sqlContext.SaveChangesAsync();
                //Form a DTO for read
                return await _animalMapper.AnimalToRead(animalToAdd);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }

        public async Task<ReadAnimalDTO> Delete(string latinicOrCommonName)
        {
            try
            {
                var animalToDel = _sqlContext.Animals.FirstOrDefault(a => a.LatinicName == latinicOrCommonName || a.CommonName == latinicOrCommonName);
                if(animalToDel is null )
                {
                    _logger.LogError("Not found");
                    throw (RequestException)new NotFoundException("Animal");
                }
                //Remove an image
                var path = BasePath.Get();
                if(File.Exists($"{path}/images/{animalToDel.ImageUrl}")) {
                    File.Delete($"{path}/images/{animalToDel.ImageUrl}");
                }
                _sqlContext.Remove(animalToDel);
                await _sqlContext.SaveChangesAsync();
                return await _animalMapper.AnimalToRead(animalToDel);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new NotFoundException("Animal");
            }
        }

        public async Task<List<ReadAnimalDTO>> Get(string? regionName)
        {
            try
            {
                //Get info
                var animals = await _sqlContext.Animals
                    .Include(a => a.Regions)
                    .ToListAsync();

                //Check for region query
                if(!(regionName is null)) {
                    animals = animals.Where(a => a.Regions.FirstOrDefault(r => r.Name == regionName) != null).ToList();
                }

                //Map dtos
                List<ReadAnimalDTO> animalInfo = new();
                foreach(var animal in animals)
                {
                    animalInfo.Add(await _animalMapper.AnimalToRead(animal));
                }
                return animalInfo;
            }
            catch(Exception e)
            {
                //Handle database exceptions
                _logger.LogError(e, e.Message);
                throw new NotImplementedException();
            }
        }

        public async Task<ReadAnimalDTO> Update(string latinicOrCommonName, CreateAnimalDTO updateAnimalPayload)
        {
            //Get the required animal
            Animal animalToUpdate = await _sqlContext.Animals
                                    .Include(a => a.Regions)
                                    .FirstOrDefaultAsync(a => a.LatinicName.ToLower() == latinicOrCommonName.ToLower() || a.CommonName.ToLower() == latinicOrCommonName.ToLower());
            if(animalToUpdate is null)
            {
                _logger.LogError("Animal not found");
                throw (RequestException)new NotFoundException("Animal");
            }
            //Check for regions
            var updateAnimal = await _animalMapper.CreateToAnimal(updateAnimalPayload);
            //Check if picture needs to be updated
            var path = BasePath.Get();
            var contentType = updateAnimalPayload.Image?.ContentType.Split("/")[1] ?? animalToUpdate.ImageUrl.Split(".")[1]; //.jpg, .png...
            if(updateAnimalPayload.Image != null) 
            {
                if(!updateAnimalPayload.Image.ContentType.StartsWith("image")) {
                    throw (RequestException)new NotFoundException("image");
                }
                using (FileStream stream = File.Create($"{updateAnimalPayload.CommonName ?? updateAnimal.CommonName}.{contentType}")) {
                    await updateAnimalPayload.Image.CopyToAsync(stream);
                }
            }
            //Update picture name if name changed
            if(File.Exists($"{path}/images/{animalToUpdate.ImageUrl}")) {
                File.Move($"{path}/images/{animalToUpdate.ImageUrl}", $"{path}/images/{updateAnimalPayload.CommonName}.{contentType}");
            }
            //Update existing animal
            animalToUpdate.ImageUrl = (animalToUpdate.CommonName == updateAnimalPayload.CommonName) ? animalToUpdate.ImageUrl : $"/images/{updateAnimalPayload.CommonName}.{contentType}";
            animalToUpdate.RingNumber = updateAnimal.RingNumber ?? animalToUpdate.RingNumber;
            animalToUpdate.LatinicName = updateAnimal.LatinicName ?? animalToUpdate.LatinicName;
            animalToUpdate.CommonName = updateAnimal.CommonName ?? animalToUpdate.CommonName;
            animalToUpdate.Regions = updateAnimal.Regions ?? animalToUpdate.Regions;
            animalToUpdate.FeedingGrounds = updateAnimal.FeedingGrounds ?? animalToUpdate.FeedingGrounds;
            //Save changes
            await _sqlContext.SaveChangesAsync();
            //Create readDTO from new animal
            return await _animalMapper.AnimalToRead(animalToUpdate);
        }
    }
}
