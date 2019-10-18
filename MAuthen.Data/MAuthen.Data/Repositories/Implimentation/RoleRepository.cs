using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;

namespace MAuthen.Data.Repositories.Implimentation
{
    public class RoleRepository: RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(MAuthenContext context): base(context){}
    }
}
