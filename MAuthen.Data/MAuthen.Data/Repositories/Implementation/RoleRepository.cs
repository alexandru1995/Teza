using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAuthen.Data.Repositories.Implementation
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        private readonly MAuthenContext _context;

        public RoleRepository(MAuthenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<Role>> GetUserServiceRoles(Guid userId, Guid ServiceId)
        {
            return await _context.UserServiceRoles
                .Where(usr => usr.UserId == userId && usr.ServiceId == ServiceId)
                .Include(r => r.Role).Select(usr => usr.Role).ToListAsync();
        }

        public async Task<IList<Role>> GetServiceRole(Guid serviceId)
        {
            return await _context.Roles.Where(r => r.ServiceId == serviceId || r.Options == RoleFlags.Default).ToListAsync();
        }

        public async Task Change(UserServiceRoles role)
        {
            var existingRole = await _context.UserServiceRoles
                .Where(r => r.ServiceId == role.ServiceId && r.UserId == role.UserId && r.Role.Options != RoleFlags.None).FirstOrDefaultAsync();
            if (existingRole == null)
            {
                throw new Exception("You don't have permission to change this role");
            }

            _context.UserServiceRoles.Remove(existingRole);
            _context.UserServiceRoles.Add(role);
            await _context.SaveChangesAsync();
        }
    }
}
