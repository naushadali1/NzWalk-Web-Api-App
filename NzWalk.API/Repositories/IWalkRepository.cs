using NzWalk.API.Models.Domain;

namespace NzWalk.API.Repositories
    {
    public interface IWalkRepository
        {
     Task<Walk> CreateAsync(Walk walk);
     Task<List<Walk>> GetAllAsync(string? filterOn=null,string? filterQuery=null , string? sortBy =null , bool isAscending = true, int pageNumber= 1, int pageSize=100);
     Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateRegionAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
        }
    }
