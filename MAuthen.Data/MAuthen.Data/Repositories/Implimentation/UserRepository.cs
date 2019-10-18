using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;

namespace MAuthen.Data.Repositories.Implimentation
{
    public class UserRepository: RepositoryBase<User>,IUserRepository
    {
        public UserRepository(MAuthenContext context): base(context){}
    }
}
