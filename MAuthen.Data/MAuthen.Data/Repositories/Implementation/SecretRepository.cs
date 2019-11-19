using System;
using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MAuthen.Data.Repositories.Implementation
{
    public class SecretRepository : RepositoryBase<Secret>, ISecretRepository
    {
        private readonly MAuthenContext _context;

        public SecretRepository(MAuthenContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Secret> GetUserSecret(Guid userId)
        {
            return await _context.Secrets.FirstOrDefaultAsync(s => s.IdUser == userId);
        }

        public async Task<string> GetRefreshToken(string username)
        {
            return await _context.Users.Where(u => u.UserName == username)
                .Include(s => s.Secret)
                .Select(s => s.Secret.RefreshToken).FirstOrDefaultAsync();
        }

        public void UpdateRefreshToken(string username, string newRefreshToken)
        {
            var userToken = _context.Users
                .Include(s => s.Secret)
                .FirstOrDefault(u => u.UserName == username);
            if (userToken != null)
                userToken.Secret.RefreshToken = newRefreshToken;
            _context.SaveChanges();
        }
    }
}
