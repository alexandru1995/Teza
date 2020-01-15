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
        private readonly MAuthenContext _context;
        public ServiceRepository(MAuthenContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ServiceSettings> AddService(string userName, Service service)
        {
            var newService = _context.Services.Add(service);

            var userId = _context.Users.Where(u => u.UserName == userName).Select(s => s.Id).FirstOrDefault();
            _context.UserServiceRoles.Add(
                new UserServiceRoles
                {
                    UserId = userId,
                    ServiceId = newService.Entity.Id,
                    RoleId = Guid.Parse("BD1FF3D5-6BE5-4660-B1BE-77284DD8B669"),
                    CreatedOn = DateTime.Now
                });
            await _context.SaveChangesAsync();
            return new ServiceSettings
            {
                client_id = newService.Entity.Id,
                issuer = newService.Entity.Issuer,
                audience = "https://localhost:5001/",
                secret = newService.Entity.ServicePassword
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
                    LastName = usr.User.LastName,
                    IsBlocked = usr.Bloked
                }).ToListAsync();
        }

        public async Task<IList<SimpleServiceModel>> GetUserServices(string username)
        {
            return await _context.UserServiceRoles
                .Include(s => s.Service)
                .Where(u => 
                    u.User.UserName == username && u.Role.Options.HasFlag(RoleFlags.Default) && u.Role.Name == "Administrator" 
                || 
                u.User.UserName == username && u.Role.Options.HasFlag(RoleFlags.None))
                .Select(s => new SimpleServiceModel

                {
                    Id = s.ServiceId,
                    Name = s.Service.Name,
                    Issuer = s.Service.Issuer,
                    CreatedOn = s.Service.CreatedOn,
                    LogoutUrl = s.Service.LogoutUrl
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

            _context.Roles.RemoveRange(
                serviceRole.Select(s => s.Role).Where(r => r.Options.HasFlag(RoleFlags.Service)));
            _context.Services.Remove(serviceRole.Select(s => s.Service).First());
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
                    ServiceId = serviceId,
                    RoleId = Guid.Parse("B942F50F-1A0D-4A7A-9010-844F95D0F7C9")
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
