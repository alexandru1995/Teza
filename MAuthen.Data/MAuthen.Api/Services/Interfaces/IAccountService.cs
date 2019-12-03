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
        JsonWebToken SignIn(IEnumerable<Claim> clames, string secretKey);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secretKey);
        Task SignOut();
    }
}
