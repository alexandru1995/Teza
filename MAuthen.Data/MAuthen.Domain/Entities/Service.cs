using System;
using System.Collections.Generic;

namespace MAuthen.Domain.Entities
{
    public class Service
    {
        private DateTime _createdOn;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public byte[] Certificate { get; set; }
        public bool HasTotp { get; set; }
        public DateTime CreatedOn
        {
            get => _createdOn;
            set => _createdOn = DateTime.UtcNow;
        }
        public ICollection<UserServiceRoles> UserServicesRoles { get; set; }
    }
}
