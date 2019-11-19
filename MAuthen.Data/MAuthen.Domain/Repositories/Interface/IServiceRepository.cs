using MAuthen.Domain.Entities;
using MAuthen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        Task<SimpleServiceModel> GetServiceById(Guid serviceId);
        Task<IList<User>> GetServiceUsers(Guid serviceId);
        Task<IList<UserRole>> GetUserRoles(Guid userId, Guid serviceId);
        
    }
}
