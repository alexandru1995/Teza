using System;
using System.Collections.Generic;
using System.Text;
using MAuthen.Domain.Entities;

namespace MAuthen.Domain.Models
{
    public class FullServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Issuer { get; set; }
        public string LogoutUrl { get; set; }
        public DateTime TokenExpirationTime { get; set; }
        public string ServicePassword { get; set; }
        public byte[] Certificate { get; set; }
        public DateTime CreatedOn { get; set; }

        public static implicit operator Service(FullServiceModel model)
        {
            return new Service
            {
                Id = model.Id,
                Name = model.Name,
                Issuer = model.Issuer,
                CreatedOn = DateTime.Now,
                LogoutUrl = model.LogoutUrl,
                Certificate = model.Certificate,
                TokenExpirationTime = model.TokenExpirationTime,
                ServicePassword = model.ServicePassword
            };
        }
    }
}