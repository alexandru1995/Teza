using System;
using System.Text;
using System.Threading.Tasks;
using Jose;
using MAuthen.Api.Helpers;
using MAuthen.Api.Models;
using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            StringValues token;
            HttpContext.Request.Form.TryGetValue("clientID", out token);
            var payload = JWT.Payload(token);
            var serviceData = JObject.Parse(payload);
            var value = serviceData.GetValue("client_id");
            var service = await _service.GetById( Guid.Parse(value.ToString()));
            try
            {
                var key = Encoding.ASCII.GetBytes(service.ServicePassword);
                JWT.Decode(token, key);
            }
            catch
            {
                return StatusCode(401);
            }
            return Redirect("https://localhost:5001/remote/"+service.Name);
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
        //TODO Get service details
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceUsers(Guid id)
        {
            return Json(await _service.GetServiceUsers(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddService(FullServiceModel service)
        {
            service.ServicePassword = PasswordGenerator.Generate(255);
            var newService = await _service.AddService(User.Identity.Name, service);
            var settings = new JsonRootModel {AuthenticationRequest = newService};
            var jsonSettings = JsonConvert.SerializeObject(settings, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore

            });
            var jsonBytes = Encoding.UTF8.GetBytes(jsonSettings);

            return File(jsonBytes, "application/octet-stream", newService + ".json");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateService(FullServiceModel service)
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