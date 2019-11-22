using System;
using System.Collections.Generic;

namespace MAuthen.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Issuer { get; set; }
        public string LogoutUrl { get; set; }
        public DateTime TokenExpirationTime { get; set; }
        public string ServicePassword { get; set; }
        public byte[] Certificate { get; set; }
        public bool HasTotp { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public ICollection<UserServiceRoles> UserServicesRoles { get; set; }
    }
}
