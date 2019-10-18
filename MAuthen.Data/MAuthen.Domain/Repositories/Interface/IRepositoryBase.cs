using System;
using System.Linq;
using System.Threading.Tasks;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetById(Guid id);
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid id);
    }
}
