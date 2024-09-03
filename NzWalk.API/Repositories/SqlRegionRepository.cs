using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Models.Domain;


namespace NzWalk.API.Repositories
    {
    public class SqlRegionRepository : IRegionRepository
        {

        public NZWalkDbContext dbContext;
        public SqlRegionRepository(NZWalkDbContext dbContext)
        {
            this.dbContext = dbContext;
            }

        public async Task<Region> CreateRegionAsync(Region region)
            {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
            }

        public async Task<Region?> DeleteRegionAsync(Guid id)
            {
         var existingRegion=  await dbContext.Regions.FirstOrDefaultAsync(s => s.Id == id);
            if (existingRegion == null) {
                return null;
                }
            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;

            }

        public async Task<List<Region>> GetAllRegionsAsync()
            {
         return await dbContext.Regions.ToListAsync();
            }

        public async Task<Region?> GetRegionsAsync(Guid id)
            {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            }

        public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
            {
          var existingData = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (existingData == null) {
                return null;
                }
            existingData.Code = region.Code;
            existingData.RegionImageUrl= region.RegionImageUrl;
            existingData.Name = region.Name;
            await dbContext.SaveChangesAsync();
            return region;  

            }
        }
    }
