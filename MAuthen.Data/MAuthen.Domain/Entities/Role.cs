using System;
using System.Collections.Generic;

namespace MAuthen.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserServiceRoles> UserServiceRoles { get; set; }
    }
}
