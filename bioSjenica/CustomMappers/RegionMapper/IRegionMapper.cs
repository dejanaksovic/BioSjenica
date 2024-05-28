using bioSjenica.DTOs.Regions;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers.CustomMappers
{
    public interface IRegionMapper
    {
        public Task<Region> CreateToRegion(CreateRegionDTO DTO);
        public Task<ReadRegionDTO> RegionToRead(Region region);
    }
}
