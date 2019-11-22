using System;
using MAuthen.Domain.Entities;

namespace MAuthen.Domain.Models
{
    public class SimpleServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Issuer { get; set; }
        public string LogoutUrl { get; set; }
        public DateTime? CreatedOn { get; set; }

        //public static implicit operator Service(SimpleServiceModel model)
        //{
        //    return  new Service
        //    {
        //        Id = model.Id,
        //        Name = model.Name,
        //        Issuer = model.Issuer,
        //        CreatedOn = DateTime.Now
                
        //    };
        //}
    }
}
