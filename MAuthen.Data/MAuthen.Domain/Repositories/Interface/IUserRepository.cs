using MAuthen.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByUsername(string username);
        Task Block(Guid userId, Guid serviceId);
        Task<Boolean> IsBlocked(Guid userId, Guid serviceId);
    }
}
