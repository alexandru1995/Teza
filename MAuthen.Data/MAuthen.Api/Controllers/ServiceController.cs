using System;
using System.Threading.Tasks;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _service;
        public ServiceController(IServiceRepository service)
        {
            _service = service;
        }

        [HttpGet("GetServices")]
        public async Task<IActionResult> GetUserServices()
        {
            var services = await _service.GetUserServices(User.Identity.Name);
            if (services.Count > 0)
            {
                return Json(services);
            }
            return StatusCode(204);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceUsers(Guid id)
        {
            return Json(await _service.GetServiceUsers(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddService(SimpleServiceModel service)
        {
            return Json(await _service.AddService(User.Identity.Name, service));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateService(SimpleServiceModel service)
        {
            await _service.Update(service);
            return StatusCode(200);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveService(Guid id)
        {
            await _service.RemoveService(id);
            return Ok();
        }

        [HttpGet("BlockUser/{serviceId}/{userId}")]
        [Authorize(Roles = "UltraAdmin, Administrator")]
        public async Task<IActionResult> BlockUser(Guid serviceId,Guid userId, [FromServices]IUserRepository userRepository)
        {
            try
            {
                await userRepository.Block(userId, serviceId);
                return Ok();
            }catch(Exception err)
            {
                return StatusCode(403,err.Message);
            }
        }

        [HttpGet("UnBlockUser/{serviceId}/{userId}")]
        [Authorize(Roles = "UltraAdmin")]
        public async Task<IActionResult> UnBlockUser(Guid serviceId, Guid userId, [FromServices]IUserRepository userRepository)
        {
            try
            {
                await userRepository.UnBlock(userId, serviceId);
                return Ok();
            }
            catch (Exception err)
            {
                return StatusCode(403, err.Message);
            }
        }
    }
}