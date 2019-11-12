using System;
using MAuthen.Api.Models;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ITokenManager _tokenManager;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordProcessor _processor;

        public IConfiguration _configuration { get; }

        public AuthorizationController(IAccountService accountService,
            ITokenManager tokenManager,
            IConfiguration configuration,
            IUserRepository user,
            IPasswordProcessor processor)
        {
            _accountService = accountService;
            _tokenManager = tokenManager;
            _configuration = configuration;
            _userRepository = user;
            _processor = processor;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody]UserSimpleModel model, [FromServices] IAccountService accountService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }
            var query = await _userRepository.SignIn(model.Username);
            var user = query.User;
            if (query == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var passwordValidation = _processor.Check(query.User.Secret.Password, model.Password);
            if (!passwordValidation.Verified)
            {
                return Unauthorized("Invalid username or password");
            }
            var clames = new List<Claim>();
            clames.Add(new Claim("FirstName", user.FirstName));
            clames.Add(new Claim("LasrName", user.LastName));
            clames.Add(new Claim("UserName", user.UserName));
            clames.Add(new Claim("Birthday", user.Birthday.ToString()));
            foreach (var role in user.UserRoles)
            {
                var test = role.Role.Name;
                clames.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }
            clames.Add(new Claim("Gender", user.Gender ? "Male" : "Female"));
            foreach (var contact in user.Contacts)
            {
                clames.Add(new Claim("Email", contact.Email));
                clames.Add(new Claim("PhoneNumber", contact.Phone));
            }


            return Json(accountService.SignIn(clames));

            //TODO generate jwt
            //return Json((UserModel)user);
        }
    }
}