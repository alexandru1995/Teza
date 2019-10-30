using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MAuthen.Api.Models.Authentication;

namespace MAuthen.Api.Services.Interfaces
{
    public interface IJwtHandler
    {
        JsonWebToken Create(IEnumerable<Claim> claims);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
