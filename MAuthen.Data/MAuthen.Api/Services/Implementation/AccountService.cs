using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAuthen.Api.Services.Implementation
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

        public JsonWebToken SignIn(IEnumerable<Claim> clames, string secretKey)
        {
            var jwt = _jwtHandler.Create(clames, secretKey);
            return jwt;
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
