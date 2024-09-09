using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NzWalk.API.Repositories
    {
    // Repository class for creating JWT tokens
    public class TokenRepository : ITokenRepository
        {
        private readonly IConfiguration configuration; // Configuration settings

        // Constructor to initialize the repository with configuration settings
        public TokenRepository(IConfiguration configuration)
            {
            this.configuration = configuration; 
            }

        // Method to create a JWT token for a user with specified roles
        public string CreateJWTToken(IdentityUser user, List<string> roles)
            {
            // Create a list of claims to include in the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email) // Add the user's email as a claim
            };

            // Add each role as a claim
            foreach (var role in roles)
                {
                claims.Add(new Claim(ClaimTypes.Role, role));
                }

            // Create a symmetric security key from the configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            // Create signing credentials using the key and HMAC-SHA256 algorithm
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token with issuer, audience, claims, expiration, and signing credentials
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"], 
                audience: configuration["Jwt:Audience"], 
                claims: claims, 
                expires: DateTime.Now.AddMinutes(19), 
                signingCredentials: credentials 
            );

            // Convert the JWT token to a string and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
