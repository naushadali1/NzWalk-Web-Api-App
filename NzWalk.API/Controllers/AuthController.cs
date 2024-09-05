using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NzWalk.API.Models.DTO;
using NzWalk.API.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace NzWalk.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
        {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository iTokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository iTokenRepository)
            {
            this.userManager = userManager;
            this.iTokenRepository = iTokenRepository;
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

        //Register method
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login( [FromBody] LoginDTO logindto)
            {
            var  userName = await userManager.FindByNameAsync(logindto.UserName);

            if (userName != null) { 
                
                var checkedPasswor= await userManager.CheckPasswordAsync(userName, logindto.Password);

                if ( checkedPasswor) {

                    // get all roles

                    var roles = await userManager.GetRolesAsync(userName);
                    // create a token for login
                    if (roles != null ) {

                      var jwtToken = iTokenRepository.CreateJWTToken(userName, roles.ToList());
                        var response = new LoginResponeDTO
                            {
                            JwtToken = jwtToken,
                            };
                        return Ok(response);
                        }

                   // return an ok  with token
                   return Ok();
                    }
                }
            return BadRequest("Login Failed Invalid credentials");
            }

        }
    }
