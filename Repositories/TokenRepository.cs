using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using TaskManagerAPI.Models.Domain;

namespace TaskManagerAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private IConfiguration configuration;

        public TokenRepository(IConfiguration configuration) {
            this.configuration = configuration;
        }

    
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //create claims
            var claims = new List<Claim>();

            claims.Add(new Claim("username", user.UserName));
            claims.Add(new Claim("userId", user.Id));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

                 var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

                 var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                 var token = new JwtSecurityToken(
                     configuration["Jwt:Issuer"],
                     configuration["Jwt:Audience"],
                     claims,
                     expires: DateTime.Now.AddMinutes(60),
                     signingCredentials: credentials);
        
                  return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
