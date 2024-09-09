using Microsoft.AspNetCore.Mvc;
using NzWalk.API.Models.Domain;
using NzWalk.API.Models.DTO;
using NzWalk.API.Repositories;

namespace NzWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRrpository imageRepository;

        // Constructor: Inject the image repository to handle uploads
        public ImageController(IImageRrpository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // POST: Upload image via form
        [HttpPost("upload")]
        public async Task<IActionResult>Upload([FromForm] ImageUploadDTO request)
        {
            // Validate the file (size, extension)
            ValidateFile(request);
            
            // Check if validation passed
            if (ModelState.IsValid)
            {
                // Map DTO to the domain model
                var domainModel = new Image
                {
                    File = request.File,
                    FileDescription = request.FileDescription,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.File.FileName,
                };

                // Upload image to the repository
                await imageRepository.Upload(domainModel);

                // Return success response with the uploaded domain model
                return Ok(domainModel);
            }

            // Return bad request with model validation errors
            return BadRequest(ModelState);
        }

        // Private method for file validation (size and extension)
        private void ValidateFile(ImageUploadDTO request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            // Validate file extension
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "File not supported");
            }

            // Validate file size (max 10 MB)
            if (request.File.Length > 10485760) 
            {
                ModelState.AddModelError("file", "File size shouldn't be greater than 10MB");
            }
        }
    }
}
