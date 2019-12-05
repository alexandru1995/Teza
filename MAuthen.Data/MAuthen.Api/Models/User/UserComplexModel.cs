using MAuthen.Api.Models.Contacts;
using System.Collections.Generic;
using System.Linq;

namespace MAuthen.Api.Models.User
{
    public class UserComplexModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string UserName { get; set; }
        public IList<ContactModel> Contacts { get; set; }

        public static implicit operator UserComplexModel(Domain.Entities.User user)
        {
            return new UserComplexModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Contacts = user.Contacts.Select(u => new ContactModel
                {
                    Email = u.Email,
                    Phone = u.Phone
                }).ToList(),
                UserName = user.UserName,
                Birthday = user.Birthday.ToString("MM/dd/yyyy"),
                Gender = user.Gender?"Man": "Woman"
            };
        }
        
    }
}
