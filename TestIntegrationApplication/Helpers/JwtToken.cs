using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Jose;
using Microsoft.Extensions.Options;
using TestIntegrationApplication.Models;

namespace TestIntegrationApplication.Helpers
{
    public class JwtToken : IJwtToken
    {
        private readonly AuthenticationRequestModel _options;
        public JwtToken(IOptions<AuthenticationRequestModel> options)
        {
            _options = options.Value;
        }
        public string Create(Dictionary<string, object> payload)
        {
            var key = Encoding.ASCII.GetBytes(_options.Secret);
            string token = JWT.Encode(payload, key , JwsAlgorithm.HS256);
            return token;
        }
    }
}
