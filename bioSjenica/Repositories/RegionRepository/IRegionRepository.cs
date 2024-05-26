﻿using bioSjenica.Models;
using bioSjenica.Payloads;

namespace bioSjenica.Repositories.RegionRepository
{
    public interface IRegionRepository
    {
        public Task<Region> CreateRegion(RegionPayload newRegion);
        public Task<List<Region>> GetAllRegions();
        public Task<Region> UpdateRegion(RegionPayload updateRegion, int id);
        public Task<Region> DeleteRegionById(int id);
    }
}
