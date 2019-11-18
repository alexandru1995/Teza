using System;

namespace MAuthen.Api.Models.Authentication
{
    public class AuthenticatedUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public JsonWebToken Tokens { get; set; }
    }
}
