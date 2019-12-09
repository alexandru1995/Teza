using Jose;
using MAuthen.Api.Models.Token;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IDistributedCache _cache;
        private readonly IRoleRepository _role;
        private readonly IAccountService _accountService;
        private readonly Models.Authentication.JwtOptions _options;
        public TokenController(IDistributedCache cache, IServiceRepository serviceRepository,
            IRoleRepository role, IAccountService accountService,
            IOptions<Models.Authentication.JwtOptions> options)
        {
            _cache = cache;
            _serviceRepository = serviceRepository;
            _role = role;
            _accountService = accountService;
            _options = options.Value;
        }

        [HttpPost]
        public async Task<string> Tokens([FromBody] AuthorizationCodeModel authorizationCode)
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
                var tokens = new TokenResponseModel
                {
                    UserId = userId,
                    IdToken = await CreateIdToken(Guid.Parse(userId), service),
                    AccessToken = CreateAccessToken(Guid.Parse(userId), service.Id)

                };
                var key = Encoding.ASCII.GetBytes(_options.SecretKey);
                return JWT.Encode(tokens, key, JwsAlgorithm.HS256);
            }
            catch
            {
                throw new UnauthorizedAccessException("Authentication error");
            }
        }

        [Authorize]
        [HttpGet("signout")]
        public async Task<IActionResult> SignOut([FromServices] IAccountService accountService)
        {
            await accountService.SignOut();
            return StatusCode(200);
        }

        private async Task<string> CreateIdToken(Guid userId, Service service)
        {
            var clams = new List<Claim>();
            var userRole = await _role.GetUserServiceRoles(userId, service.Id);
            foreach (var role in userRole)
            {
                clams.Add(new Claim(ClaimTypes.Name, userId.ToString()));
                clams.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            return _accountService.SignIn(clams, service.ServicePassword).AccessToken;
        }

        private string CreateAccessToken(Guid userId, Guid serviceId)
        {
            var clams = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString()),
                new Claim("client_id", serviceId.ToString())
            };
            return _accountService.SignIn(clams, _options.SecretKey).AccessToken;
        }
    }
}