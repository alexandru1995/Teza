using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MAuthen.Api.Models;
using MAuthen.Api.Services.Interfaces;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _user;
        private readonly IPasswordProcessor _processor;
        public UserController(IUserRepository user, IPasswordProcessor processor)
        {
            _user = user;
            _processor = processor;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Json(_user.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Json(await _user.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]UserModel model)
        {
            try
            {
                await _user.Create(new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Blocked = false,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Secret = new Secret{
                        Password = _processor.Hash(model.Password)
                    }
                    
                });
            }
            catch(Exception err)
            {
                switch (err.GetType().Name)
                {
                    case "DbUpdateException": return StatusCode(409,"Error This email alredy exist.");
                }
            }
            
            return Json(StatusCode(201, "Successful creation"));
        }

        [HttpPut]
        public async Task<IActionResult> Update(User model)
        {
            await _user.Update(model);
            return Json(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _user.Delete(id);
            return StatusCode(202,"Delited");
        }
    }
}