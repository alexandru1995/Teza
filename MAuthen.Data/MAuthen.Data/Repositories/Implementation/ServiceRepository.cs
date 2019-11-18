using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;

namespace MAuthen.Data.Repositories.Implementation
{
    public class ServiceRepository: RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(MAuthenContext context): base(context){}
    }
}
