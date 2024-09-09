using NzWalk.API.Data;
using NzWalk.API.Models.Domain;

namespace NzWalk.API.Repositories
    {
    // Implementation of IImageRepository for handling image uploads
    public class LocalImageRepository : IImageRrpository
        {
        // Field to access web hosting environment details
        private readonly IWebHostEnvironment webHostEnvironment;

        // Field to access HTTP context for retrieving request details
        private readonly IHttpContextAccessor httpContextAccessor;

        // Field to interact with the database context
        private readonly NZWalkDbContext dbContext;

        // Constructor to initialize the repository with dependencies
        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalkDbContext dbContext)
            {
            this.webHostEnvironment = webHostEnvironment;  
            this.httpContextAccessor = httpContextAccessor; 
            this.dbContext = dbContext;                     
            }

        // Method to upload an image
        public async Task<Image> Upload(Image image)
            {
            // Define the local file path where the image will be stored
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            // Upload image to the local file path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // Create the URL path to access the uploaded image
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath; // Set the URL path to the image object

            // Save image details to the database
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            // Return the image object with updated details
            return image;
            }
        }
    }
