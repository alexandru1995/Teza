using System;

namespace MAuthen.Domain.Entities
{
    public class UserServiceRoles
    {
        private DateTime _createdOn;
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = DateTime.UtcNow; } }
    }
}
