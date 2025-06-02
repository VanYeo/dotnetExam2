using Microsoft.AspNetCore.Identity;

namespace dotnetExam2.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
