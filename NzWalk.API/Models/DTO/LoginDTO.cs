using System.ComponentModel.DataAnnotations;

namespace NzWalk.API.Models.DTO
    { 
    public class LoginDTO
        { 
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        }
    
    }
