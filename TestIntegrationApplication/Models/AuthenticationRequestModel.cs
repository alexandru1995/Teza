﻿namespace TestIntegrationApplication.Models
{
    public class AuthenticationRequestModel
    {
        public string Client_Id { get; set; }
        public string Issuer { set; get; }
        public string Audience { set; get; }
        public string Secret { set; get; }
        public string ServerSecret { get; set; }
    }
}
