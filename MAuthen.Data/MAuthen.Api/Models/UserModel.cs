using MAuthen.Api.Models.Contacts;
using MAuthen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAuthen.Api.Models
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public string Birthday { get; set; }
        public string UserName { get; set; }
        public IList<ContactModel> Contacts { get; set; }
        public string Password { get; set; }

        public static implicit operator UserModel(User user)
        {
            return new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Contacts = user.Contacts.Select(u => new ContactModel
                {
                    Email = u.Email,
                    Phone = u.Phone,
                    Id = u.Id
                }).ToList(),
                UserName = user.UserName,
                Birthday = user.Birthday.ToString("MM/dd/yyyy"),
                Gender = user.Gender
            };
        }
        public static implicit operator User(UserModel user)
        {
            return new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Contacts = user.Contacts.Select(u => new Domain.Entities.Contacts
                {
                    Email = u.Email,
                    Phone = u.Phone,
                    Id =  u.Id
                }).ToList(),
                Secret = new Secret{Password = user.Password},
                UserName = user.UserName,
                Birthday = DateTime.Parse(user.Birthday),
                Gender = user.Gender
            };
        }
    }
}
