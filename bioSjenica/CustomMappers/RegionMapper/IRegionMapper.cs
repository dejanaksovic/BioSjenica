using bioSjenica.DTOs.Regions;
using bioSjenica.Models;

namespace bioSjenica.CustomMappers
{
    public interface IRegionMapper
    {
        public Task<Region> CreateToRegion(CreateRegionDTO DTO);
        public ReadRegionDTO RegionToRead(Region region);
        public List<ReadRegionDTO> RegionToReadList (List<Region> regions);
    }
}
