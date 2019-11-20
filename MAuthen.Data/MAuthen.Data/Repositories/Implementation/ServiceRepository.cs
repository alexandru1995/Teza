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

        public async Task<SimpleServiceModel> AddService(string userName, Service service)
        {
            var newService = _context.Services.Add(service);
            var role = _context.Add(new Role {Name = "SuperAdmin"});
            var userId = _context.Users.Where(u => u.UserName == userName).Select(s => s.Id).FirstOrDefault();
            _context.UserServiceRoles.Add(
                new UserServiceRoles
                {
                    UserId = userId,
                    ServiceId = newService.Entity.Id,
                    RoleId = role.Entity.Id,
                    CreatedOn = DateTime.Now
                });
            await _context.SaveChangesAsync();
            return new SimpleServiceModel
            {
                Id = newService.Entity.Id,
                Name = newService.Entity.Name,
                Domain = newService.Entity.Domain,
                CreatedOn = newService.Entity.CreatedOn
            };
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
        //TODO Get full user with role
        public async Task<IList<User>> GetServiceUsers(Guid serviceId)
        {
            return await _context.UserServiceRoles.Where(usr => usr.ServiceId == serviceId)
                .Include(u => u.User).Select(usr => usr.User).ToListAsync();
        }

        public async Task<IList<SimpleServiceModel>> GetUserServices(string username)
        {
            return await _context.UserServiceRoles
                .Include(s => s.Service)
                .Where(u => u.User.UserName == username)
                .Select(s => new SimpleServiceModel
                {
                    Id = s.ServiceId,
                    Name = s.Service.Name,
                    Domain = s.Service.Domain,
                    CreatedOn = s.Service.CreatedOn
                }).ToListAsync();
        }

        public async Task RemoveService(Guid serviceId)
        {
            var serviceRole = await _context.UserServiceRoles
                .AsNoTracking()
                .Where(r => r.ServiceId == serviceId)
                .Include(r => r.Role)
                .Include(s => s.Service)
                .Select(s => new
                {
                    s.Role,
                    s.Service
                }).ToListAsync();

            _context.Roles.RemoveRange(serviceRole.Select(s=>s.Role));
            _context.Services.Remove(serviceRole.Select(s=>s.Service).First());
            await _context.SaveChangesAsync();

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
