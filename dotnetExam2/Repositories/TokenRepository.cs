using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnetExam2.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            // Create claims from system.security.claims
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // read secret key from appsettings.json, convert to byte array, wrap in symmetricsecuritykey
            // used to sign token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            // generate signing credentials with HmacSha256 hashing algorithm
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // create token with the digital signature
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            // convert token to compatible string format to return in api response
            // /store in authorisation header
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
