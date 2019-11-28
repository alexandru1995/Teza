using System.Collections.Generic;
using System.Security.Claims;

namespace TestIntegrationApplication.Helpers
{
    public interface IJwtToken
    {
        string Create(string returnUrl);
        string GetAuthorizationCode(string Token);
    }
}
