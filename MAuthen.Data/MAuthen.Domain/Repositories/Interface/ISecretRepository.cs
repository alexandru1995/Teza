using System;
using System.Threading.Tasks;
using MAuthen.Domain.Entities;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface ISecretRepository: IRepositoryBase<Secret>
    {
        Task<Secret> GetUserSecret(Guid userId);
        Task<string> GetRefreshToken(string username);
        void UpdateRefreshToken(string username, string newRefreshToken);
    }
}
