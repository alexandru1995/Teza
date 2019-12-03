using System.Security.Claims;

namespace TestIntegrationApplication.Services.Interfaces
{
    public interface IJwtHandler
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secretKey);
    }
}
