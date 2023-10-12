using Microsoft.AspNetCore.Identity;

namespace TaskManagerAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
