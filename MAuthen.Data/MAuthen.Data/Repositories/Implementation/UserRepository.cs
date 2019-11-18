using System;
using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<string> GetRefreshToken(string username)
        {
            return await _context.Users.Where(u => u.UserName == username)
                .Include(s => s.Secret)
                .Select(s => s.Secret.RefreshToken).FirstOrDefaultAsync();
        }

        public async void UpdateRefreshToken(string username, string newRefreshToken)
        {
            var userToken = _context.Users
                .Include(s => s.Secret)
                .FirstOrDefault(u => u.UserName == username);
            if (userToken != null) 
                userToken.Secret.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();
        }
    }
}
