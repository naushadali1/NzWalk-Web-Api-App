using System.ComponentModel.DataAnnotations.Schema;

namespace NzWalk.API.Models.Domain
    {
    public class Image
        {
        public Guid  Id { get; set; }
        public string FileName { get; set; }
        [NotMapped]
        public IFormFile File{ get; set; }
        public string FileExtension { get; set; }
        public string? FileDescription { get; set; }
        public string FilePath { get; set; }
        public long FileSizeInBytes { get; set; }
    }
    }
