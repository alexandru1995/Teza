using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        [HttpPost]
        public async Task<IActionResult> Add(Role model)
        {
            await _role.Create(model);
            return StatusCode(201, "Created");
        }
    }
}