using System;
using MAuthen.Api.Models;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

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
        public async Task<IActionResult> SignIn([FromBody]UserSimpleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request");
            }
            var user = await _userRepository.SignIn(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var passwordValidation = _processor.Check(user.Secret.Password, model.Password);
            if (!passwordValidation.Verified)
            {
                return Unauthorized("Invalid username or password");
            }


            //TODO generate jwt
            return Json((UserModel)user);
        }
    }
}