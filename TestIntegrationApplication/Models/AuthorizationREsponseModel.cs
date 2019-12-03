using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestIntegrationApplication.Models
{
    public class AuthorizationResponseModel
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
    }
}
