using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MAuthen.Data.Repositories.Implementation
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly MAuthenContext _context;
        public UserRepository(MAuthenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task Block(Guid id, Guid serviceId)
        {
            var user = await _context.UserServiceRoles.Where(
                u => u.UserId == id &&
                u.ServiceId == serviceId &&
                !u.Role.Options.HasFlag(RoleFlags.None))
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("You don't hav permission to block this user;");
            }
            user.Bloked = true;
            _context.UserServiceRoles.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsBlocked(Guid userId, Guid serviceId)
        {
            return await _context.UserServiceRoles
                .Where(u => u.UserId == userId && u.ServiceId == serviceId)
                .Select(s => s.Bloked).FirstOrDefaultAsync();
        }

        public async Task UnBlock(Guid id, Guid serviceId)
        {
            var user = await _context.UserServiceRoles.Where(
                u => u.UserId == id &&
                u.ServiceId == serviceId &&
                u.Bloked)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new Exception("This user isn't blocked");
            }
            user.Bloked = false;
            _context.UserServiceRoles.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
