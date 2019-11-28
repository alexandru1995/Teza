using Jose;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using TestIntegrationApplication.Models;

namespace TestIntegrationApplication.Helpers
{
    public class JwtToken : IJwtToken
    {
        private AuthenticationRequestModel _options;
        public JwtToken(IOptions<AuthenticationRequestModel> options)
        {
            _options = options.Value;
        }
        public string Create(string returnUrl)
        {
            var payload = new Dictionary<string, object>()
            {
                {"scope", "Authorization"},
                {"response_type", "code"},
                {"client_id", _options.Client_Id},
                {"redirect_uri", returnUrl}
            };
            var key = Encoding.ASCII.GetBytes(_options.Secret);
            string token = JWT.Encode(payload, key , JwsAlgorithm.HS256);
            return token;
        }

        public string GetAuthorizationCode(string token)
        {
            var payload = JWT.Payload(token);
            
            try
            {
                var key = Encoding.ASCII.GetBytes(_options.ServerSecret);
                JWT.Decode(token, key);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Invalid tiken sifnature");
            }
            var serviceData = JObject.Parse(payload);
            var code = serviceData.GetValue("AuthorizationCode");
            return code.ToString();
        }
    }
}
