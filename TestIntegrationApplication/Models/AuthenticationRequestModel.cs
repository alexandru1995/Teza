using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestIntegrationApplication.Models
{
    public class AuthenticationRequestModel
    {
        public string Client_Id { get; set; }
        public string Issuer { set; get; }
        public string Audience { set; get; }
        public string Secret { set; get; }
    }
}
