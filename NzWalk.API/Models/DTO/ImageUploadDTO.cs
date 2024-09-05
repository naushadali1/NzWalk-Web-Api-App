using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NzWalk.API.Models.DTO
    {
    public class ImageUploadDTO
        {
     
        public string FileName { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public string? FileDescription { get; set; }
        }
    }
