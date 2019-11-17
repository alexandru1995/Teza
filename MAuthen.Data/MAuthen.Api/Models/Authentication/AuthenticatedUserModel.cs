using System;

namespace MAuthen.Api.Models.Authentication
{
    public class AuthenticatedUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
