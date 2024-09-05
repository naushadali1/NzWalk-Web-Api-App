using Microsoft.AspNetCore.Http;
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

        public ImageController(IImageRrpository imageRepository)
            {
            this.imageRepository = imageRepository;
            }

        [HttpPost("upload")] // Explicitly specify the HTTP method
        public async Task<IActionResult> Upload([FromForm] ImageUploadDTO request)
            {
            ValidateFile(request);
            if (ModelState.IsValid)
                {
                // Convert DTO to domain model
                var domainModel = new Image
                    {
                    File = request.File,
                    FileDescription = request.FileDescription,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.File.FileName,
                    };

                // Image repository to upload image
                await imageRepository.Upload(domainModel);

                return Ok(domainModel);
                }

            return BadRequest(ModelState);
            }

        private void ValidateFile(ImageUploadDTO request) // Change visibility to private
            {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
                {
                ModelState.AddModelError("file", "File not supported");
                }

            if (request.File.Length > 10485760)
                {
                ModelState.AddModelError("file", "File size shouldn't be greater than 10MB");
                }
            }
        }
    }
