using MAuthen.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IContactRepository : IRepositoryBase<Contacts>
    {
        Task<IList<Contacts>> AddContacts(string username, Contacts contacts);
        Task<Contacts> GetContactByUserId(Guid userId);
    }
}
