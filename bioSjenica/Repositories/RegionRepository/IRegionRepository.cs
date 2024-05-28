using bioSjenica.DTOs.Regions;
using bioSjenica.Models;

namespace bioSjenica.Repositories.RegionRepository
{
    public interface IRegionRepository
    {
        public Task<ReadRegionDTO> CreateRegion(CreateRegionDTO newRegion);
        public Task<List<ReadRegionDTO>> GetAllRegions();
        public Task<ReadRegionDTO> UpdateRegion(CreateRegionDTO updateRegion, string regionName);
        public Task<ReadRegionDTO> DeleteRegionByName(string regionName);
    }
}
