using bioSjenica.Data;
using bioSjenica.Models;
using bioSjenica.Payloads;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Repositories.RegionRepository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly SqlContext _sqlContext;
        private readonly ILogger<RegionRepository> _logger;
        public RegionRepository(SqlContext context, ILogger<RegionRepository> logger)
        {
            _sqlContext = context;
            _logger = logger;
        }

        public async Task<Region> CreateRegion(RegionPayload newRegion)
        {

            var toAddRegion = new Region();

            toAddRegion.Name = newRegion.Name;
            toAddRegion.Area = newRegion.Area;
            toAddRegion.ProtectionType = newRegion.ProtectionType;
            toAddRegion.Villages = newRegion.Villages;

            _sqlContext.Add(toAddRegion);

            try
            {
                await _sqlContext.SaveChangesAsync();
                return toAddRegion;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Region> DeleteRegionById(int id)
        {
            var regionToDelete = _sqlContext.Regions.FirstOrDefault(r => r.Id == id);
            if (regionToDelete is null)
                return null;
            _sqlContext.Remove(regionToDelete);
            try
            {
                await _sqlContext.SaveChangesAsync();
                return regionToDelete;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Region>> GetAllRegions()
        {
            try
            {
                return await _sqlContext.Regions.ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Region> UpdateRegion(RegionPayload updateRegion, int id)
        {
            var regionToUpdate = _sqlContext.Regions.FirstOrDefault(r => r.Id == id);
            regionToUpdate.Name = updateRegion.Name;
            regionToUpdate.Area = updateRegion.Area;
            regionToUpdate.Villages = updateRegion.Villages;
            regionToUpdate.ProtectionType = updateRegion.ProtectionType;

            try
            {
                await _sqlContext.SaveChangesAsync();
                return regionToUpdate;
            }

            catch(Exception e)
            {
                return null;
            }
        }
    }
}
