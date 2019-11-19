using MAuthen.Api.Models;
using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public IConfiguration _configuration { get; }

        public AuthorizationController(
            IConfiguration configuration,
            IUserRepository user,
            IPasswordProcessor processor,
            ISecretRepository secret,
            IRoleRepository role)
        {
            _configuration = configuration;
            _userRepository = user;
            _processor = processor;
            _secret = secret;
            _role = role;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]UserSimpleModel model, [FromServices] IAccountService accountService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
            var clams = new List<Claim>
            {
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim("Birthday", user.Birthday.ToString(CultureInfo.InvariantCulture))
            };
            clams.Add(new Claim("Gender", user.Gender ? "Male" : "Female"));
            if (user.Contacts != null)
            {
                var contact = user.Contacts.FirstOrDefault();
                clams.Add(new Claim("Email", contact?.Email??""));
                clams.Add(new Claim("PhoneNumber", contact?.Phone??""));

            }
            var userRole = await _role.GetUserRoles(user.Id);
            foreach (var role in userRole)
            {
                clams.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            var refresh = GenerateRefreshToken();
            _userRepository.UpdateRefreshToken(user.UserName, refresh);

            return Json(new AuthenticatedUserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Tokens = new JsonWebToken
                {
                    AccessToken = accountService.SignIn(clams).AccessToken,
                    RefreshToken = refresh
                }
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody]JsonWebToken model, [FromServices] IAccountService accountService)
        {
            var principal = accountService.GetPrincipalFromExpiredToken(model.AccessToken);
            var username = principal.Identity.Name;
            if (username == null)
                return BadRequest();
            var refresh = await _userRepository.GetRefreshToken(username);
            if (refresh != null && model.RefreshToken == refresh)
            {
                var newRefreshToken = GenerateRefreshToken();
                _userRepository.UpdateRefreshToken(principal.Identity.Name, newRefreshToken);
                return Json(new JsonWebToken
                {
                    AccessToken = accountService.SignIn(principal.Claims).AccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("signout")]
        public async Task SignOut([FromServices] IAccountService accountService)
        {
            _userRepository.UpdateRefreshToken(User.Identity.Name, null);
            await accountService.SignOut();
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}