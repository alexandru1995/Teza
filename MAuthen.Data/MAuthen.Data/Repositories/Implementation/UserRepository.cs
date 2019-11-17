using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Models;
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
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<UserRole> SignIn(string username)
        {
            return await _context.UserRoles
                .Include(u => u.User)
                    .ThenInclude(c => c.Contacts)
                    .Include(u => u.User.Secret)
                .Include(r => r.Role)
                .Where(u => u.User.UserName == username).FirstOrDefaultAsync();
        }

    }
}
