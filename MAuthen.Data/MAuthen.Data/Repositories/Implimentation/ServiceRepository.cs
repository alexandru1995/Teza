using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;

namespace MAuthen.Data.Repositories.Implimentation
{
    public class ServiceRepository: RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(MAuthenContext context): base(context){}
    }
}
