using System;
using System.Collections.Generic;
using System.Text;

namespace MAuthen.Domain.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<ServiceRole> ServiceRoles { get; set; }
    }
}
