using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Models.Domain;

namespace NzWalk.API.Repositories
    {
    // Implementation of IRegionRepository using SQL database
    public class SqlRegionRepository : IRegionRepository
        {
        // Field to access the database context
        private readonly NZWalkDbContext dbContext;

        // Constructor to initialize the repository with the database context
        public SqlRegionRepository(NZWalkDbContext dbContext)
            {
            this.dbContext = dbContext; 
            }

        // Method to create a new region
        public async Task<Region> CreateRegionAsync(Region region)
            {
            // Add the new region to the database context
            await dbContext.Regions.AddAsync(region);

  
            await dbContext.SaveChangesAsync();

            // Return the created region
            return region;
            }

        // Method to delete a region by its ID
        public async Task<Region?> DeleteRegionAsync(Guid id)
            {
            // Find the existing region by its ID
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(s => s.Id == id);


            if (existingRegion == null)
                {
                return null;
                }

            // Remove the existing region from the database context
            dbContext.Regions.Remove(existingRegion);

           
            await dbContext.SaveChangesAsync();

            // Return the removed region
            return existingRegion;
            }

        // Method to retrieve all regions
        public async Task<List<Region>> GetAllRegionsAsync()
            {
            // Retrieve and return all regions from the database
            return await dbContext.Regions.ToListAsync();
            }

        // Method to retrieve a specific region by its ID
        public async Task<Region?> GetRegionsAsync(Guid id)
            {
            // Find and return the region by its ID
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            }

        // Method to update a region by its ID
        public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
            {
            // Find the existing region by its ID
            var existingData = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

            // If the region does not exist, return null
            if (existingData == null)
                {
                return null;
                }

            // Update the existing region with new values
            existingData.Code = region.Code;
            existingData.RegionImageUrl = region.RegionImageUrl;
            existingData.Name = region.Name;

           
            await dbContext.SaveChangesAsync();

            // Return the updated region
            return region;
            }
        }
    }
