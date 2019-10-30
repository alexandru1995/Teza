using System.Threading.Tasks;
using MAuthen.Domain.Models;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> SignIn(string email, string password);
    }
}
