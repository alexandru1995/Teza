namespace MAuthen.Api.Models.Authentication
{
    public class SignInModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServiceName { get; set; }
    }
}
