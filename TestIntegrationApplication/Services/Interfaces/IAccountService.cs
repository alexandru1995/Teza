using System.Security.Claims;
using System.Threading.Tasks;

namespace TestIntegrationApplication.Services.Interfaces
{
    public interface IAccountService
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secretKey);
        Task SignOut();
    }
}
