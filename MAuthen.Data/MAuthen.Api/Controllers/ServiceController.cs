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
            return Json(await _service.GetUserServices(User.Identity.Name));
        }

        //public async Task<IActionResult> GetServiceUsers(Guid serviceId)
        //{
        //    return null;
        //}
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
    }
}