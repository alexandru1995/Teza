using System;

namespace MAuthen.Domain.Entities
{
    public class UserServiceRoles
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Boolean Bloked { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
