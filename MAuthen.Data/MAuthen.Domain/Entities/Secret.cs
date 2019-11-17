using System;

namespace MAuthen.Domain.Models
{
    public class Secret
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string TotpSecret { get; set; }
        public string RefreshToken { get; set; }
        public Guid IdUser { get; set; }
        public User User { get; set; }
    }
}
