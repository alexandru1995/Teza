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

        public async Task<Guid> GetServiceIdByName(string name)
        {
            return await _context.Services.Where(s => s.Name == name).Select(s => s.Id).FirstOrDefaultAsync();
        }

        public async Task<IList<User>> GetServiceUsers(Guid serviceId)
        {
            return await _context.UserServiceRoles.Where(usr => usr.ServiceId == serviceId)
                .Include(u => u.User).Select(usr => usr.User).ToListAsync();
        }

        //public async Task<IList<UserRole>> GetUserRoles(Guid userId, Guid serviceId)
        //{
        //    //var userRole = await _context.Roles
        //    //    .Include(s => s.ServiceRoles.Where(sr => sr.IdService == serviceId))
        //    //    .Include(u => u.UserRoles.Where(ur => ur.IdUser == userId)).ToListAsync();
        //    //var userRoles =  _context.Roles.
                                    
        //    throw new NotImplementedException();
        //}
    }
}
