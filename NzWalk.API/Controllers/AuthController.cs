using Microsoft.AspNetCore.Identity;  
using Microsoft.AspNetCore.Mvc;        
using NzWalk.API.Models.DTO;          
using NzWalk.API.Repositories;       

namespace NzWalk.API.Controllers
    {
    [Route("api/[controller]")]       
    [ApiController]                    
    public class AuthController : ControllerBase
        {
        
        private readonly UserManager<IdentityUser> userManager;        // Handles user-related operations like creating and managing users.
        private readonly ITokenRepository iTokenRepository;            // Manages token-related operations such as JWT generation.

        // Constructor that accepts and assigns the injected dependencies
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository iTokenRepository)
            {
            this.userManager = userManager;               
            this.iTokenRepository = iTokenRepository;    
            }

        // Register endpoint for creating a new user.
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDTO authRegisterDTO)
            {
            // Create a new IdentityUser with the provided username and email.
            var identityUser = new IdentityUser
                {
                UserName = authRegisterDTO.Username,       
                Email = authRegisterDTO.Username,          
                };

            // Attempt to create the user with the provided password.
            var identityResult = await userManager.CreateAsync(identityUser, authRegisterDTO.Password);

            // If user creation succeeded:
            if (identityResult.Succeeded)
                {
                // If roles are provided, assign them to the newly created user.
                if (authRegisterDTO.Roles != null && authRegisterDTO.Roles.Any())
                    {
                    foreach (var role in authRegisterDTO.Roles) 
                        {
                        identityResult = await userManager.AddToRoleAsync(identityUser, role);
                        if (identityResult.Succeeded)
                            {
                            // Return success message once user is registered and roles assigned.
                            return Ok("User registered successfully");
                            }
                        }
                    }
                }

            // If user creation failed, return a bad request with an error message.
            return BadRequest("User registration failed!");
            }

        // Login endpoint for authenticating an existing user.
        [HttpPost]
        [Route("Login")]  
        public async Task<IActionResult> Login([FromBody] LoginDTO logindto)
            {
            // Find the user by username (login name).
            var userName = await userManager.FindByNameAsync(logindto.UserName);

            // If the user exists:
            if (userName != null)
                {
                // Check if the provided password matches the stored password.
                var checkedPassword = await userManager.CheckPasswordAsync(userName, logindto.Password);

              
                if (checkedPassword)
                    {
                    
                    var roles = await userManager.GetRolesAsync(userName);

                    // Generate a JWT token if the user has roles.
                    if (roles != null)
                        {
                        //create token
                        var jwtToken = iTokenRepository.CreateJWTToken(userName, roles.ToList()); 

                        // Create a response DTO with the generated JWT token.
                        var response = new LoginResponeDTO
                            {
                            JwtToken = jwtToken,  
                            };

                        // Return the JWT token to the client.
                        return Ok(response);
                        }

                    // If no roles, return an empty success message.
                    return Ok();
                    }
                }
            return BadRequest("Login Failed: Invalid credentials");
            }
        }
    }
