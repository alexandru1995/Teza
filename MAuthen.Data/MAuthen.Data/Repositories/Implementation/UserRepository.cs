using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MAuthen.Data.Repositories.Implementation
{
    public class UserRepository: RepositoryBase<User>,IUserRepository
    {
        private readonly MAuthenContext _dbContext;
        public UserRepository(MAuthenContext context) : base(context)
        {
            _dbContext = context;
        }
        public async Task<User> SignIn(string username, string password)
        {
            return await _dbContext.Users.Include(u => u.Secret).Where(u => u.UserName == username).FirstOrDefaultAsync();
        }
    }
}
