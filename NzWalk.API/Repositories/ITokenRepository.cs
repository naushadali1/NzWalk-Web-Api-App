using Microsoft.AspNetCore.Identity;

namespace NzWalk.API.Repositories
    {
    public interface ITokenRepository
        {
        string CreateJWTToken(IdentityUser user, List<string> roles);
        }
    }
