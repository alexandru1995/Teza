using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAuthen.Data.Repositories.Implementation
{
    public class ContactRepository : RepositoryBase<Contacts>, IContactRepository
    {
        private MAuthenContext _context;
        public ContactRepository(MAuthenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IList<Contacts>> AddContacts(string username, Contacts contacts)
        {
            var user = await _context.Users.Where(u => u.UserName == username)
                .Include(c => c.Contacts).FirstOrDefaultAsync();
            user.Contacts.Add(contacts);
            await _context.SaveChangesAsync();
            return user.Contacts.ToList();
        }

        public async Task<Contacts> GetContactByUserId(Guid userId)
        {
            return await _context.Contacts
                .Include(c => c.Email)
                .Include(p => p.Phone)
                .Where(c => c.User.Id == userId).FirstOrDefaultAsync();
        }
    }
}
