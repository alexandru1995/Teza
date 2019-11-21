using System;
using System.Linq;
using MAuthen.Api.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MAuthen.Api.Models.Authentication;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _role;
        public RoleController(IRoleRepository role)
        {
            _role = role;
        }

        public IActionResult Get()
        {
            return Json(_role.GetAll());
        }

        [HttpGet("ServiceRoles/{serviceId}")]
        public async Task<IActionResult> GetServiceRole(Guid serviceId)
        {
            var roles = await _role.GetServiceRole(serviceId);
            return  Json(roles.Select(s => (RoleModel)s));
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoleModel model)
        {
            await _role.Create(model);
            return StatusCode(201, "Created");
        }

        [HttpPost("Change")]
        public async Task<IActionResult> Change(ChangeRoleModel model)
        {
            try
            {
                await _role.Change(model);
                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(403, e.Message);
            }
            
            
        }
    }
}