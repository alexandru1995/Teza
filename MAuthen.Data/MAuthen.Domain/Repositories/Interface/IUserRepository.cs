using MAuthen.Domain.Entities;
using System.Threading.Tasks;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByUsername(string username);
    }
}
