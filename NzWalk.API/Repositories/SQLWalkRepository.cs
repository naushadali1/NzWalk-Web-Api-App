using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Models.Domain;

namespace NzWalk.API.Repositories
    {
    // Repository class for managing Walk entities with SQL database
    public class SQLWalkRepository : IWalkRepository
        {
        // Field to access the database context
        private readonly NZWalkDbContext dbContext;

        // Constructor to initialize the repository with the database context
        public SQLWalkRepository(NZWalkDbContext dbContext)
            {
            this.dbContext = dbContext; // Assign the provided database context
            }

        // Method to create a new Walk entry
        public async Task<Walk> CreateAsync(Walk walk)
            {
            // Add the new Walk entity to the database context
            await dbContext.Walks.AddAsync(walk);

            // Save changes to the database
            await dbContext.SaveChangesAsync();

            // Return the created Walk entity
            return walk;
            }

        // Method to delete a Walk entry by its ID
        public async Task<Walk?> DeleteAsync(Guid id)
            {
            // Find the Walk entity with the specified ID
            var walkFound = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            // If the Walk entity is not found, return null
            if (walkFound == null)
                {
                return null;
                }

            // Remove the found Walk entity from the database context
            dbContext.Walks.Remove(walkFound);

            // Save changes to the database
            await dbContext.SaveChangesAsync();

            // Return the deleted Walk entity
            return walkFound;
            }

        // Method to retrieve all Walk entities with optional filtering, sorting, and pagination
        public async Task<List<Walk>> GetAllAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 100)
            {
            // Query the Walk entities and include related entities like Difficulty and Region
            var walks = dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
                {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                    // Filter walks by Name if specified
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                    }
                }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
                {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                    // Sort by Name if specified
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                    }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                    {
                    // Sort by LengthInKm if specified
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                    }
                }

            // Pagination
            var skipResult = (pageNumber - 1) * pageSize;
            // Skip over the items for previous pages and take the number of items specified by pageSize
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
            }

        // Method to retrieve a specific Walk entity by its ID
        public async Task<Walk?> GetByIdAsync(Guid id)
            {
            // Find and return the Walk entity with the specified ID, including related entities
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
            }

        // Method to update a Walk entry by its ID
        public async Task<Walk?> UpdateRegionAsync(Guid id, Walk walk)
            {
            // Find the existing Walk entity by its ID
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            // If the Walk entity is not found, return null
            if (existingWalk == null)
                {
                return null;
                }

            // Update the existing Walk entity with new values from the provided Walk entity
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.LengthInKm = walk.LengthInKm;

            // Save changes to the database
            await dbContext.SaveChangesAsync();

            // Return the updated Walk entity
            return existingWalk;
            }
        }
    }
