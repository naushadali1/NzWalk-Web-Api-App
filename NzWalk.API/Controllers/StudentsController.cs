using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NzWalk.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
        {
        [HttpGet]
        public IActionResult GetAllStudents() { string[] students = new string[] { "Ali", "Naushad", "Haider", "Hassan" };
            return Ok(students);
            }
        }
    }
