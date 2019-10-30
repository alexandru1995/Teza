using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Models;

namespace MAuthen.Api.Models
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public static implicit operator UserModel(User user)
        {
            return new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
