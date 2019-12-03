using System.Security.Claims;
using System.Threading.Tasks;
using TestIntegrationApplication.Services.Interfaces;

namespace TestIntegrationApplication.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly ITokenManager _tokenManager;

        public AccountService(IJwtHandler jwtHandler, ITokenManager tokenManager)
        {
            _jwtHandler = jwtHandler;
            _tokenManager = tokenManager;
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secretKey)
        {
            return _jwtHandler.GetPrincipalFromExpiredToken(token, secretKey);
        }

        public async Task SignOut()
        {
           await _tokenManager.DeactivateCurrentAsync();
        }
    }
}
