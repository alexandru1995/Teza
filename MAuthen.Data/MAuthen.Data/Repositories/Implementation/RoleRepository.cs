using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MAuthen.Data.Repositories.Implementation
{
    public class RoleRepository: RepositoryBase<Role>, IRoleRepository
    {
        private readonly MAuthenContext _context;
        public RoleRepository(MAuthenContext context): base(context){}

        public async Task<IList<Role>> GetUserRoles(Guid userId)
        {
            return await _context.UserRoles.Include(d => d.Role)
                .Where(r => r.IdUser == userId)
                .Select(r => r.Role).ToListAsync();
        }
    }
}
