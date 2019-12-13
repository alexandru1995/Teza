using Jose;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text;
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
