using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MAuthen.Api.Services.Implimentation
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions _options;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly SecurityKey _securityKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;

        public JwtHandler(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
        }

        public JsonWebToken Create(IEnumerable<Claim> claims)
        {
            var nowUtc = DateTime.UtcNow;
            var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            var expires = nowUtc.AddMinutes(_options.ExpiryMinutes);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Subject = new ClaimsIdentity(claims),
                Audience = _options.Audience,
                IssuedAt = DateTime.UtcNow,
                Expires = expires,
                SigningCredentials = _signingCredentials,
                EncryptingCredentials = new EncryptingCredentials(_securityKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes128CbcHmacSha256)
            };

            var encriptToken = _jwtSecurityTokenHandler.CreateJwtSecurityToken(descriptor);
            var token = _jwtSecurityTokenHandler.WriteToken(encriptToken);
            return new JsonWebToken
            {
                AccessToken = token
            };
            
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                TokenDecryptionKey = _securityKey,
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            

            if (jwtSecurityToken == null || !jwtSecurityToken.InnerToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

    }
}
