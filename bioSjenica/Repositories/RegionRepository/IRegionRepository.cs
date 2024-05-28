using bioSjenica.Models;
using bioSjenica.Payloads;

namespace bioSjenica.Repositories.RegionRepository
{
    public interface IRegionRepository
    {
        public Task<Region> CreateRegion(RegionDTO newRegion);
        public Task<List<Region>> GetAllRegions();
        public Task<Region> UpdateRegion(RegionDTO updateRegion, int id);
        public Task<Region> DeleteRegionById(int id);
    }
}
