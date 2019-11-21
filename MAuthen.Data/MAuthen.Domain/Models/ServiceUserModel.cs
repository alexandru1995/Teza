using System;

namespace MAuthen.Domain.Models
{
    public class ServiceUserModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public Boolean IsBlocked { get; set; }
    }
}
