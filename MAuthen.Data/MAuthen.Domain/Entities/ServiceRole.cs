using System;
using System.Collections.Generic;
using System.Text;

namespace MAuthen.Domain.Models
{
    public class ServiceRole
    {
        public Guid IdService { get; set; }
        public Service Service { get; set; }
        public Guid IdRole { get; set; }
        public Role Role { get; set; }
    }
}
