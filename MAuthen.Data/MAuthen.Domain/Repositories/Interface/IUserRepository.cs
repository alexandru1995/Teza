using System;
using System.Threading.Tasks;
using MAuthen.Domain.Models;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByUsername(string username);
        Task<string> GetRefreshToken(string username);
        void UpdateRefreshToken(string username, string newRefreshToken);
    }
}
