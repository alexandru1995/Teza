using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Entities;

namespace MAuthen.Api.Models
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public Guid? ServiceId { get; set; }
        public string Name { get; set; }

        public static implicit operator Role(RoleModel model)
        {
            return new Role
            {
                Id = model.Id,
                Name = model.Name,
                ServiceId = model.ServiceId
            };
        }

        public static implicit operator RoleModel(Role model)
        {
            return  new RoleModel
            {
                Id = model.Id,
                Name = model.Name,
                ServiceId = model.ServiceId
            };
        }
    }
}
