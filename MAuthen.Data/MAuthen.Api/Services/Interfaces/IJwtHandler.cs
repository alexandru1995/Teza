﻿using MAuthen.Api.Models.Authentication;
using System.Collections.Generic;
using System.Security.Claims;

namespace MAuthen.Api.Services.Interfaces
{
    public interface IJwtHandler
    {
        JsonWebToken Create(IEnumerable<Claim> claims, string secretKey);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secretKey);
    }
}
