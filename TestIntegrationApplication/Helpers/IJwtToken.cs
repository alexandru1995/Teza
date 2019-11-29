using System.Collections.Generic;

namespace TestIntegrationApplication.Helpers
{
    public interface IJwtToken
    {
        string Create(Dictionary<string, object> payload);
    }
}
