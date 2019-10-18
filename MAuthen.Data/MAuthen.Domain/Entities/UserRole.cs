using System;
using System.Collections.Generic;
using System.Text;

namespace MAuthen.Domain.Models
{
    
    public class UserRole
    {
        private DateTime _createdOn;
        public Guid IdRole { get; set; }
        public Role Role { get; set; }
        public Guid IdUser { get; set; }
        public User User { get; set; }
        public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = DateTime.UtcNow; } }
    }
}
