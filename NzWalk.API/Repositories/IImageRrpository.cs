using NzWalk.API.Models.Domain;

namespace NzWalk.API.Repositories
    {
    public interface IImageRrpository
        {
        Task<Image> Upload(Image image);
        }
    }
