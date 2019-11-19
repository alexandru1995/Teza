using System;
using System.Collections.Generic;

namespace MAuthen.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime Birthday { get; set; }
        public ICollection<Contacts> Contacts { get; set; }
        public string UserName { get; set; }
        public bool Blocked { get; set; }
        public DateTime BlockedTime { get; set; }
        public Secret Secret { get; set; }
        public ICollection<UserServiceRoles> UserServiceRoles { get; set; }
    }
}
