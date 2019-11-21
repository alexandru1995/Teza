using System;
using MAuthen.Domain.Entities;

namespace MAuthen.Api.Models.Authentication
{
    public class ChangeRoleModel
    {
        public Guid RoleId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid UserId { get; set; }

        public static implicit operator UserServiceRoles(ChangeRoleModel model)
        {
            return new UserServiceRoles
            {
                RoleId = model.RoleId,
                UserId = model.UserId,
                ServiceId = model.ServiceId
            };
        }
    }
}
