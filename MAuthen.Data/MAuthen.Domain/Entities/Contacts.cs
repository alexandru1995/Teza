using System;
using System.Collections.Generic;
using System.Text;

namespace MAuthen.Domain.Entities
{
    public class Contacts
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
