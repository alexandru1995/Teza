using System;
using System.Collections.Generic;

namespace MAuthen.Api.Models.Contacts
{
    public class ContactModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public static implicit operator ContactModel(Domain.Entities.Contacts model)
        {
            return new ContactModel
            {
                Id = model.Id,
                Email = model.Email,
                Phone = model.Phone
            };
        }
        public static implicit operator Domain.Entities.Contacts(ContactModel model)
        {
            return new Domain.Entities.Contacts
            {
                Id = model.Id,
                Email = model.Email,
                Phone = model.Phone
            };
        }
    }
}
