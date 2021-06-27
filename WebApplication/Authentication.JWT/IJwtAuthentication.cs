using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication.Authentication.JWT
{
    public interface IJwtAuthentication
    {
        Task<JwtAuthenticationResult> GenerateTokens(string email, Claim[] claims);
    }
}