using System;
using System.Collections.Generic;

namespace MAuthen.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Blocked { get; set; }
        public DateTime BlockedTime { get; set; }
        public Secret Secret { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserService> UserServices { get; set; }
    }
}
