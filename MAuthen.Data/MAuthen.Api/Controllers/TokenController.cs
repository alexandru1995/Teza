using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Jose;
using MAuthen.Api.Models.Token;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IDistributedCache _cache;
        public TokenController(IDistributedCache cache, IUserRepository userRepository, IServiceRepository serviceRepository)
        {
            _cache = cache;
            _userRepository = userRepository;
            _serviceRepository = serviceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Tokens([FromBody] AuthorizationCodeModel authorizationCode)
        {

            var payload = JWT.Payload(authorizationCode.Token);
            var codeData = JObject.Parse(payload);
            var clientId = codeData.GetValue("client_id");
            
            var service = await _serviceRepository.GetById(Guid.Parse(clientId.ToString()));

            try
            {
                var serviceKey = Encoding.ASCII.GetBytes(service.ServicePassword);
                JWT.Decode(authorizationCode.Token, serviceKey);
                var code = codeData.GetValue("AuthorizationCode").ToString();
                var userId = await _cache.GetStringAsync(code);
                await _cache.RemoveAsync(code);

            }
            catch
            {
                return StatusCode(401);
            }
            return null;
        }

        private string CreateIdToken(Guid UserId, Service service)
        {
            var payload = new Dictionary<string, object>
            {
            };
            return null;
        }

        private string CreateAccessToken([FromServices] IAccountService accountService)
        {
            return null;
        }

    }
}