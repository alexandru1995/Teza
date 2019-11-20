using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAuthen.Data.Repositories.Implementation
{
    public class RoleRepository: RepositoryBase<Role>, IRoleRepository
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
    }
}
