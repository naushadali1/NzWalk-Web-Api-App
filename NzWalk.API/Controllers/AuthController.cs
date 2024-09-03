using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NzWalk.API.Models.DTO;
using System.Reflection.Metadata.Ecma335;

namespace NzWalk.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
        {
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager)
            {
            this.userManager = userManager;
            }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDTO authRegisterDTO)
            {
            var identityUser = new IdentityUser
                {
                UserName = authRegisterDTO.Username,
                Email = authRegisterDTO.Username,
                };


            var identityResult = await userManager.CreateAsync(identityUser, authRegisterDTO.Password);

            if (identityResult.Succeeded)
                {
                // Add roles to this user
                if (authRegisterDTO.Roles != null && authRegisterDTO.Roles.Any() ) {
                    foreach (var role in authRegisterDTO.Roles)
                    {
                        identityResult = await userManager.AddToRoleAsync(identityUser, role);
                        if (identityResult.Succeeded)
                        {
                         return   Ok("User registered succefully");
                        }
                    }

                    }
                }
         return   BadRequest("User registration Failed!");
            }

        }
    }
