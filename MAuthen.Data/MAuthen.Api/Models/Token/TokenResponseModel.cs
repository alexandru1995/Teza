using System;

namespace MAuthen.Api.Models.Token
{
    public class TokenResponseModel
    {
        public string UserId { get; set; }
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
    }
}
