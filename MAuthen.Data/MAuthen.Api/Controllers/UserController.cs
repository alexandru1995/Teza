using MAuthen.Api.Models;
using MAuthen.Api.Models.Contacts;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
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
        
        public async Task<JsonResult> Get()
        {
            var user = await _user.GetUserByUsername(User.Identity.Name);
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            return Json(user, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver
            });
        }

        [HttpGet("users")]
        [Authorize(Roles = "UltraAdmin")]
        public IActionResult GetAll()
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                model.Password = _processor.Hash(model.Password);
                await _user.Create(model);
            }
            catch(Exception err)
            {
                switch (err.GetType().Name)
                {
                    case "DbUpdateException": return StatusCode(409,"Error This email already exist.");
                }
            }
            
            return Json(StatusCode(201, "Successful creation"));
        }

        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact([FromBody] ContactModel model)
        {
            return Json(await _user.AddContacts(User.Identity.Name, model));
        }

        [HttpPost("UpdateContact")]
        public async Task<IActionResult> UpdateContact([FromBody] ContactModel model)
        {
            return Json(await _user.UpdateContacts( model));
        }


        [HttpDelete("DeleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            try
            {
                await _user.deleteContacts(id);
                return StatusCode(204);
            }
            catch(Exception err)
            {
                return StatusCode(500);
            }
            
        }
        [HttpPut]
        public async Task<JsonResult> Update(User model)
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