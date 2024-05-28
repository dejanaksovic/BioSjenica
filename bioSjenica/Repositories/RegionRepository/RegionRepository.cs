using bioSjenica.CustomMappers.CustomMappers;
using bioSjenica.Data;
using bioSjenica.DTOs.Regions;
using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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
        public async Task<ReadRegionDTO> DeleteRegionById(string regionName)
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
            ReadRegionDTO regionToRetun = new()
            {
                Name = regionToDelete.Name,
                Area = regionToDelete.Area,
                Villages = regionToDelete.Villages,
                ProtectionType = regionToDelete.ProtectionType,
                Animals = regionToDelete.Animals,
                Plants = regionToDelete.Plants,
                FeedingGrounds = regionToDelete.FeedingGrounds,
            };
            return regionToRetun;
        }
        public async Task<List<ReadRegionDTO>> GetAllRegions()
        {
            List<Region> regions = await _sqlContext.Regions.ToListAsync();
            List<ReadRegionDTO> regionsToReturn = new();
            foreach(Region region in regions)
            {
                regionsToReturn.Add(new()
                {
                    Name = region.Name,
                    Area = region.Area,
                    Villages = region.Villages,
                    ProtectionType = region.ProtectionType,
                    Animals = region.Animals,
                    Plants = region.Plants,
                    FeedingGrounds = region.FeedingGrounds
                });
            }
            return regionsToReturn;
        }
        public async Task<ReadRegionDTO> UpdateRegion(CreateRegionDTO updateRegion, string regionName)
        {
            Region regionToUpdate = _sqlContext.Regions.FirstOrDefault(r => r.Name == regionName);
            regionToUpdate.Name = updateRegion.Name;
            regionToUpdate.Area = updateRegion.Area;
            regionToUpdate.Villages = updateRegion.Villages;
            regionToUpdate.ProtectionType = updateRegion.ProtectionType;

            try
            {
                await _sqlContext.SaveChangesAsync();
                return new ReadRegionDTO();
            }

            catch(Exception e)
            {
                return null;
            }
        }
    }
}
