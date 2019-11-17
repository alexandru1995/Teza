using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MAuthen.Api.Services.Implimentation
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

        public JsonWebToken SignIn(IEnumerable<Claim> clames)
        {
            var jwt = _jwtHandler.Create(clames);
            return jwt;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            return _jwtHandler.GetPrincipalFromExpiredToken(token);
        }

        public async Task SignOut()
        {
           await _tokenManager.DeactivateCurrentAsync();
        }
    }
}
