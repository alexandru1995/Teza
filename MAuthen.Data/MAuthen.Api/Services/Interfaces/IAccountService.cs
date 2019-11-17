using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MAuthen.Api.Models.Authentication;

namespace MAuthen.Api.Services.Interfaces
{
    public interface IAccountService
    {
        JsonWebToken SignIn(IEnumerable<Claim> clames);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task SignOut();
        //void RevokeRefreshToken(string token);
    }
}
