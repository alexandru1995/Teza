using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services.Interfaces;

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
