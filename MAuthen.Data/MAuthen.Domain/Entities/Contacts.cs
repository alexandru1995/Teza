using System;
using System.Collections.Generic;

namespace MAuthen.Domain.Entities
{
    public class Contacts
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public ICollection<ContactEmail> Email { get; set; }
        public ICollection<ContactPhone> Phone { get; set; }
    }
}
