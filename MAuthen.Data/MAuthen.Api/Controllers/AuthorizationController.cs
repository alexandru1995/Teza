using Jose;
using MAuthen.Api.Helpers;
using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordProcessor _processor;
        private readonly ISecretRepository _secret;
        private readonly IRoleRepository _role;
        private readonly IServiceRepository _service;
        private readonly IContactRepository _contact;
        private readonly Models.Authentication.JwtOptions _options;
        private readonly IDistributedCache _cache;

        public AuthorizationController(
            IUserRepository user,
            IPasswordProcessor processor,
            IContactRepository contact,
            IServiceRepository service,
            ISecretRepository secret,
            IOptions<Models.Authentication.JwtOptions> options,
            IRoleRepository role,
            IDistributedCache cache)
        {
            _userRepository = user;
            _processor = processor;
            _contact = contact;
            _service = service;
            _secret = secret;
            _role = role;
            _options = options.Value;
            _cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]SignInModel model, [FromServices] IAccountService accountService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //TODO replace in separate function 
            var user = await _userRepository.GetUserByUsername(model.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var userPassword = await _secret.GetUserSecret(user.Id);
            var passwordValidation = _processor.Check(userPassword.Password, model.Password);
            if (!passwordValidation.Verified)
            {
                return Unauthorized("Invalid username or password");
            }
            var serviceId = await _service.GetServiceIdByName(model.ServiceName);
            if (await _userRepository.IsBlocked(user.Id, serviceId))
            {
                return StatusCode(403, "You are blocked on this service");
            }
            ///////////////
            
            var claims = new List<Claim>
            {
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim("Birthday", user.Birthday.ToString(CultureInfo.InvariantCulture)),
                new Claim("Gender", user.Gender ? "Male" : "Female")
            };
            await _service.AddUserToService(serviceId, user.Id);
            var userRole = await _role.GetUserServiceRoles(user.Id, serviceId);
            foreach (var role in userRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            var userContact = await _contact.GetContactByUserId(user.Id);
            if (user.Contacts != null)
            {
                claims.Add(new Claim("Email", userContact.Where(e => e.Email != null).Select(c => c.Email).FirstOrDefault() ?? ""));
                claims.Add(new Claim("PhoneNumber", userContact.Where(p => p.Phone != null).Select(p => p.Phone).FirstOrDefault() ?? ""));
            }

            var refresh = PasswordGenerator.Generate(32);
            _secret.UpdateRefreshToken(user.UserName, refresh);

            return Json(new AuthenticatedUserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Tokens = new JsonWebToken
                {
                    AccessToken = accountService.SignIn(claims, _options.SecretKey).AccessToken,
                    RefreshToken = refresh
                }
            });
        }

        [HttpPost("RemoteSignin")]
        public async Task<IActionResult> RemoteSignin([FromBody] SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //TODO replace in separate function 
            var user = await _userRepository.GetUserByUsername(model.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var userPassword = await _secret.GetUserSecret(user.Id);
            var passwordValidation = _processor.Check(userPassword.Password, model.Password);
            if (!passwordValidation.Verified)
            {
                return Unauthorized("Invalid username or password");
            }
            var serviceId = await _service.GetServiceIdByName(model.ServiceName);
            if (await _userRepository.IsBlocked(user.Id, serviceId))
            {
                return StatusCode(403, "You are blocked on this service");
            }
            ///////////////

            var service = await _service.GetById(serviceId);
            await _service.AddUserToService(serviceId, user.Id);
            return Json(await ResponseOnRemoteLogin(user.Id, service.Issuer));
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody]JsonWebToken model, [FromServices] IAccountService accountService)
        {
            var principal = accountService.GetPrincipalFromExpiredToken(model.AccessToken, _options.SecretKey);
            var username = principal.Identity.Name;
            if (username == null)
                return BadRequest();
            var refresh = await _secret.GetRefreshToken(username);
            if (refresh != null && model.RefreshToken == refresh)
            {
                _secret.UpdateRefreshToken(principal.Identity.Name, refresh);
                return Json(new JsonWebToken
                {
                    AccessToken = accountService.SignIn(principal.Claims, _options.SecretKey).AccessToken,
                    RefreshToken = refresh
                });
            }

            Response.Headers.Remove("Token-Expired");
            Response.Headers.Add("Invalid-refresh", "true");
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("signout")]
        public async Task SignOut([FromServices] IAccountService accountService)
        {
            _secret.UpdateRefreshToken(User.Identity.Name, null);
            await accountService.SignOut();
        }

        private async Task<ServiceAuthorizationResponse> ResponseOnRemoteLogin(Guid userId, string issuer)
        {
            var authorizationCode = "_" + Guid.NewGuid();
            await _cache.SetStringAsync(authorizationCode, userId.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            var payload = new Dictionary<string, object>()
            {
                {"AuthorizationCode", authorizationCode}
            };
            var key = Encoding.ASCII.GetBytes(_options.SecretKey);

            return new ServiceAuthorizationResponse
            {
                AuthorizationCode = JWT.Encode(payload, key, JwsAlgorithm.HS256),
                ServiceUrl = issuer + "Account/OnLogin"
            };
        }
    }
}