using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MAuthen.Data.Repositories.Implementation
{
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        private MAuthenContext _context;
        public ServiceRepository(MAuthenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SimpleServiceModel> GetServiceById(Guid serviceId)
        {
            return await _context.Services.Where(s => s.Id == serviceId)
                .Select(ns => new SimpleServiceModel {
                    Id = ns.Id,
                    Name = ns.Name,
                    Domain = ns.Domain,
                    CreatedOn = ns.CreatedOn
                }).FirstOrDefaultAsync();
        }

        public async Task<IList<User>> GetServiceUsers(Guid serviceId)
        {
            return await _context.UserServices
                .Include(s => s.Service)
                .Include(u => u.User)
                .Where(us => us.IdService == serviceId).Select(s => s.User)
                .ToListAsync();
        }

        public Task<IList<UserRole>> GetUserRoles(Guid userId, Guid serviceId)
        {
            throw new NotImplementedException();
        }
    }
}
