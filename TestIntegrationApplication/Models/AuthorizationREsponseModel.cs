namespace TestIntegrationApplication.Models
{
    public class AuthorizationResponseModel
    {
        public string UserId { get; set; }
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
    }
}
