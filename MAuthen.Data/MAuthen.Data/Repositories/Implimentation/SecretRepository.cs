using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;

namespace MAuthen.Data.Repositories.Implimentation
{
    public class SecretRepository : RepositoryBase<Secret>, ISecretRepository
    {
        public SecretRepository(MAuthenContext context): base(context){}
    }
}
