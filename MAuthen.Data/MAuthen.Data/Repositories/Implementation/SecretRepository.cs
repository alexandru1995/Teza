using System;
using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Models;
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
    }
}
