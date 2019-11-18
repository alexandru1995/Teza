using System;
using System.Threading.Tasks;
using MAuthen.Domain.Models;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface ISecretRepository: IRepositoryBase<Secret>
    {
        Task<Secret> GetUserSecret(Guid userId);
    }
}
