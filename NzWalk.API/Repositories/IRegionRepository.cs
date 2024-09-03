
using NzWalk.API.Models.Domain;

namespace NzWalk.API.Repositories
    {
    public interface IRegionRepository
        {

        Task<List<Region>> GetAllRegionsAsync();
        Task<Region?> GetRegionsAsync(Guid id);
        Task<Region> CreateRegionAsync(Region region);
        Task<Region?> DeleteRegionAsync(Guid id);
        Task<Region?> UpdateRegionAsync(Guid id , Region region);

           
        }
    }
