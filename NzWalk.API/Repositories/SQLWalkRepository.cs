using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Models.Domain;

namespace NzWalk.API.Repositories
    {
    public class SQLWalkRepository : IWalkRepository
        {
        private readonly NZWalkDbContext dbContext;

        public SQLWalkRepository(NZWalkDbContext dbContext)
        {
            this.dbContext = dbContext;
            }

        public async Task<Walk> CreateAsync(Walk walk)
            {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
            }

        public async Task<Walk?> DeleteAsync(Guid id)
            {
          var walkFound = await  dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walkFound == null) {
                return null;
                }
            dbContext.Walks.Remove(walkFound);
            await dbContext.SaveChangesAsync();
            return walkFound;
            
            }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100) {
        //  return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
           var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) ==false) {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)) {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                    }
                }

            // Sorting

            if (string.IsNullOrWhiteSpace(sortBy) == false ) {
                
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                    walks = isAscending? walks.OrderBy(x => x.Name): walks.OrderByDescending(x=>x.Name);
                    }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);

                    }


                }

            //  Pagenation
            var skipResult = (pageNumber -1) * pageSize;
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();

            }

        public async Task<Walk?> GetByIdAsync(Guid id)
            {
          return  await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x=> x.Id ==id);
            }

        public async Task<Walk?> UpdateRegionAsync(Guid id, Walk walk)
            {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id ==id);
            if (existingWalk == null)
                {
                return null;
                }
           existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.LengthInKm = walk.LengthInKm;
           await dbContext.SaveChangesAsync();
            return existingWalk;


            }
        }
    }
