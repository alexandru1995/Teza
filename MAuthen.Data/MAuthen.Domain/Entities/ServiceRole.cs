using System;

namespace MAuthen.Domain.Entities
{
    public class ServiceRole
    {
        public Guid IdService { get; set; }
        public Service Service { get; set; }
        public Guid IdRole { get; set; }
        public Role Role { get; set; }
    }
}
