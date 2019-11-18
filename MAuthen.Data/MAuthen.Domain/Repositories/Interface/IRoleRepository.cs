using MAuthen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        Task<IList<Role>> GetUserRoles(Guid userId);
    }
}
