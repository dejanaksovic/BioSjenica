using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs.Regions;
using bioSjenica.Exceptions;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;
using bioSjenica.Utilities;

namespace bioSjenica.Repositories.RegionRepository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly SqlContext _sqlContext;
        private readonly ILogger<RegionRepository> _logger;
        private readonly IRegionMapper _regionMapper;

        public RegionRepository(SqlContext context, ILogger<RegionRepository> logger, IRegionMapper regionMapper)
        {
            _sqlContext = context;
            _logger = logger;
            _regionMapper = regionMapper;
        }

        public bool RegionExists(string name) {
            if(_sqlContext.Regions.FirstOrDefault(r => r.Name == name) is null) return false;

            return true;
        }
        public async Task<ReadRegionDTO> CreateRegion(CreateRegionDTO newRegion)
        {
            Region regionToAdd = await _regionMapper.CreateToRegion(newRegion);
            
            //Save changes
            _sqlContext.Add(regionToAdd);
            try {
                await _sqlContext.SaveChangesAsync();
            }
            catch(Exception e) {
                throw new RequestException("Db error", errorCodes.INTERNAL_ERROR);
            }

            return _regionMapper.RegionToRead(regionToAdd);
        }
        public async Task<ReadRegionDTO> DeleteRegionByName(string regionName)
        {
            Region? regionToDelete = _sqlContext.Regions.FirstOrDefault(r => r.Name == regionName);
            if(regionToDelete is null) throw new RequestException("Region not found", errorCodes.NOT_FOUND);

            _sqlContext.Regions.Remove(regionToDelete);
            try {
                await _sqlContext.SaveChangesAsync();
            }
            catch(Exception e) {
                throw new RequestException("Db error", errorCodes.INTERNAL_ERROR);
            }
            //Read dto to return
            return _regionMapper.RegionToRead(regionToDelete);
        }
        public async Task<List<ReadRegionDTO>> GetAllRegions()
        {
            List<Region> regions = await _sqlContext.Regions
                                        .Include(r => r.Animals)
                                        .Include(r => r.FeedingGrounds)
                                        .Include(r => r.Plants)
                                        .ToListAsync();

            return _regionMapper.RegionToReadList(regions);
        }
        public async Task<ReadRegionDTO> UpdateRegion(CreateRegionDTO updateRegionPayload, string regionName)
        {
            var regionToUpdate = _sqlContext.Regions
                                    .Include(r => r.Animals)
                                    .FirstOrDefault(r => r.Name == regionName);

            if(regionToUpdate is null) throw new RequestException("Region not found", errorCodes.NOT_FOUND);

            var updateRegion = await _regionMapper.CreateToRegion(updateRegionPayload);
            
            regionToUpdate.Name ??= updateRegion.Name;
            regionToUpdate.Area = updateRegionPayload.Area;
            regionToUpdate.Villages ??= updateRegion.Villages;
            regionToUpdate.ProtectionType ??= updateRegion.ProtectionType;
            regionToUpdate.FeedingGrounds ??= updateRegion.FeedingGrounds;
            regionToUpdate.Plants ??= updateRegion.Plants;
            regionToUpdate.Animals ??= updateRegion.Animals;

            try
            {
                await _sqlContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new RequestException("Db error", errorCodes.INTERNAL_ERROR);
            }

            return _regionMapper.RegionToRead(updateRegion);
        }
    }
}
