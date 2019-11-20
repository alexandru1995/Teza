using MAuthen.Domain.Entities;
using MAuthen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        Task<IList<ServiceUserModel>> GetServiceUsers(Guid serviceId);
        Task<Guid> GetServiceIdByName(string name);
        Task<SimpleServiceModel> AddService(string userName, Service service);
        Task<IList<SimpleServiceModel>> GetUserServices(string username);
        Task RemoveService(Guid serviceId);
        Task AddUserToService(Guid serviceId, Guid UserId);
    }
}
