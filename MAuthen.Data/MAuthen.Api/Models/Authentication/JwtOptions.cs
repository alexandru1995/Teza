using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAuthen.Api.Models.Authentication
{
    public class JwtOptions
    {
        public string Issuer { set; get; }
        public string Audience { set; get; }
        public int ExpiryMinutes { get; set; }
        public string SecretKey { set; get; }
    }
}
