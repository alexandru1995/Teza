using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MAuthen.Api.Services.Implementation
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions _options;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public JwtHandler(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public JsonWebToken Create(IEnumerable<Claim> claims, string secretKey)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(_options.ExpiryMinutes);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Subject = new ClaimsIdentity(claims),
                Audience = _options.Audience,
                IssuedAt = DateTime.UtcNow,
                Expires = expires,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                EncryptingCredentials = new EncryptingCredentials(securityKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes128CbcHmacSha256)
            };

            var encryptToken = _jwtSecurityTokenHandler.CreateJwtSecurityToken(descriptor);
            var token = _jwtSecurityTokenHandler.WriteToken(encryptToken);
            return new JsonWebToken
            {
                AccessToken = token
            };
            
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secretKey)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                TokenDecryptionKey = securityKey,
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            

            if (jwtSecurityToken == null || !jwtSecurityToken.InnerToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
