using System;

namespace MAuthen.Domain.Entities
{
    public class UserService
    {
        private DateTime _createdOn;
        public Guid IdUser { get; set; }
        public User User { get; set; }
        public Guid IdService { get; set; }
        public Service Service { get; set; }
        public bool Consent { get; set; }
        public DateTime CreatedOn {
            get { return _createdOn; }
            set { _createdOn = DateTime.UtcNow; }
        }
    }
}
