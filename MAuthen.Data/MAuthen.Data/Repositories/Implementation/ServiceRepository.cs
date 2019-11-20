﻿using System;
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
        private readonly MAuthenContext _context;
        public ServiceRepository(MAuthenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<SimpleServiceModel> AddService(string userName, Service service)
        {
            var newService = _context.Services.Add(service);

            var role = _context.Roles.Add(new Role {Name = "SuperAdmin"});
            _context.Roles.Add(new Role {Name = "user"});
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

        public async Task<Guid> GetServiceIdByName(string name)
        {
            return await _context.Services.Where(s => s.Name == name).Select(s => s.Id).FirstOrDefaultAsync();
        }
        //TODO Get full user with role
        public async Task<IList<ServiceUserModel>> GetServiceUsers(Guid serviceId)
        {
            return await _context.UserServiceRoles.Where(usr => usr.ServiceId == serviceId)
                .Include(r => r.Role)
                .Include(u => u.User)
                .Select(usr => new ServiceUserModel
                {
                    Id = usr.UserId,
                    Role = usr.Role.Name,
                    FirstName = usr.User.FirstName,
                    LastName = usr.User.LastName
                }).ToListAsync();
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
        //TODO Insert User to client service
        public async Task AddUserToService(Guid serviceId, Guid userId)
        {
            var userIsAssigned =
                _context.UserServiceRoles.FirstOrDefault(s => s.ServiceId == serviceId && s.UserId == userId);
            if (userIsAssigned == null)
            {
                _context.UserServiceRoles.Add(new UserServiceRoles
                {
                    UserId = userId,
                    ServiceId = serviceId
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
