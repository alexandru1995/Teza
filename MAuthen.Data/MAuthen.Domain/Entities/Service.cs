using System;
using System.Collections.Generic;
using System.Text;

namespace MAuthen.Domain.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Certificate { get; set; }
        public bool HasTotp { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<UserService> UserServices { get; set; }
        public ICollection<ServiceRole> ServiceRoles { get; set; }
    }
}
