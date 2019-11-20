using MAuthen.Api.Models.Contacts;
using MAuthen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MAuthen.Api.Models
{
    public class UserModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage ="First Name is required")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gender is required")]
        public bool Gender { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Birthday is required")]
        public string Birthday { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        public IList<ContactModel> Contacts { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
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
                    Phone = u.Phone
                }).ToList(),
                Secret = new Secret{Password = user.Password},
                UserName = user.UserName,
                Birthday = DateTime.Parse(user.Birthday),
                Gender = user.Gender
            };
        }
    }
}
