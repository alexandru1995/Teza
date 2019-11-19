﻿using MAuthen.Domain.Entities;
using System.Threading.Tasks;

namespace MAuthen.Domain.Repositories.Interface
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByUsername(string username);
        //Task<string> GetRefreshToken(string username);
        //void UpdateRefreshToken(string username, string newRefreshToken);
        //Task<IList<Contacts>> AddContacts(string username, Contacts contacts);
        //Task<Contacts> UpdateContacts(Contacts contacts);
        //Task deleteContacts(Guid id);
    }
}
