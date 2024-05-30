using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs.Regions;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ReadRegionDTO> CreateRegion(CreateRegionDTO newRegion)
        {
            Region regionToAdd = await _regionMapper.CreateToRegion(newRegion);
            
            //Save changes
            _sqlContext.Add(regionToAdd);
            await _sqlContext.SaveChangesAsync();

            ReadRegionDTO regionToSend = await _regionMapper.RegionToRead(regionToAdd);

            return regionToSend;
        }
        public async Task<ReadRegionDTO> DeleteRegionByName(string regionName)
        {
            Region regionToDelete = _sqlContext.Regions.FirstOrDefault(r => r.Name == regionName);
            //TODO: Handle not found exception
            if(regionToDelete is null)
            {
                _logger.LogError("Region not found");
                throw new NotImplementedException();
            }
            _sqlContext.Regions.Remove(regionToDelete);
            await _sqlContext.SaveChangesAsync();
            //Read dto to return
            ReadRegionDTO regionToRetun = await _regionMapper.RegionToRead(regionToDelete);
            return regionToRetun;
        }
        public async Task<List<ReadRegionDTO>> GetAllRegions()
        {
            List<Region> regions = await _sqlContext.Regions
                                        .Include(r => r.Animals)
                                        .Include(r => r.FeedingGrounds)
                                        .Include(r => r.Plants)
                                        .ToListAsync();
            List<ReadRegionDTO> regionsToReturn = new();
            foreach(Region region in regions)
            {
                regionsToReturn.Add(await _regionMapper.RegionToRead(region));
            }
            return regionsToReturn;
        }
        public async Task<ReadRegionDTO> UpdateRegion(CreateRegionDTO updateRegionPayload, string regionName)
        {
            var regionToUpdate = _sqlContext.Regions
                                    .Include(r => r.Animals)
                                    .FirstOrDefault(r => r.Name == regionName);

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
                return await _regionMapper.RegionToRead(updateRegion);
            }

            catch(Exception e)
            {
                return null;
            }
        }
    }
}
