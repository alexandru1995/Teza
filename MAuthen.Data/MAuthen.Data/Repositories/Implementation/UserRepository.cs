using System;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAuthen.Data.Repositories.Implementation
{
    public class UserRepository: RepositoryBase<User>,IUserRepository
    {
        private readonly MAuthenContext _context;
        public UserRepository(MAuthenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users
                .Include(c => c.Contacts)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        //public async Task<string> GetRefreshToken(string username)
        //{
        //    return await _context.Users.Where(u => u.UserName == username)
        //        .Include(s => s.Secret)
        //        .Select(s => s.Secret.RefreshToken).FirstOrDefaultAsync();
        //}

        //public void UpdateRefreshToken(string username, string newRefreshToken)
        //{
        //    var userToken = _context.Users
        //        .Include(s => s.Secret)
        //        .FirstOrDefault(u => u.UserName == username);
        //    if (userToken != null) 
        //        userToken.Secret.RefreshToken = newRefreshToken;
        //    _context.SaveChanges();
        //}

        //public async Task<IList<Contacts>> AddContacts(string username, Contacts contacts)
        //{
        //    var user = await _context.Users.Where(u => u.UserName == username)
        //        .Include(c => c.Contacts).FirstOrDefaultAsync();
        //    user.Contacts.Add(contacts);
        //    await _context.SaveChangesAsync();
        //    return user.Contacts.ToList();
        //}

        //public async Task<Contacts> UpdateContacts(Contacts contact)
        //{
        //    var currentContact = await _context.Contacts.AsNoTracking().Where(c => c.Id == contact.Id).FirstOrDefaultAsync();
        //    if (currentContact == null)
        //    {
        //        throw new ApplicationException("Contact not exist");
        //    }
        //    try
        //    {
        //        _context.Contacts.Update(contact);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        throw new ApplicationException("Error on insert contact");
        //    }

        //    return contact;
        //}

        //public async Task deleteContacts(Guid id)
        //{
        //    var contact = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        //    if (contact == null)
        //    {
        //        throw new ApplicationException("Contact not exist");
        //    }

        //    _context.Contacts.Remove(contact);
        //    await _context.SaveChangesAsync();
        //}
    }
}
