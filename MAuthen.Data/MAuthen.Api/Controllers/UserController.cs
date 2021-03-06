﻿using MAuthen.Api.Models;
using MAuthen.Api.Models.Contacts;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using MAuthen.Api.Models.User;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _user;
        private readonly IPasswordProcessor _processor;
        private readonly IContactRepository _contact;
        public UserController(IUserRepository user, IPasswordProcessor processor, IContactRepository contact)
        {
            _user = user;
            _contact = contact;
            _processor = processor;
        }
        
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var user = await _user.GetUserByUsername(User.Identity.Name);
            user.Contacts= await _contact.GetContactByUserId(user.Id);
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            return Json(user, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpGet("users")]
        [Authorize(Roles = "UltraAdmin")]
        public IActionResult GetAll()
        {
            return Json(_user.GetAll());
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetById()
        {
            var id = User.Identity.Name;
            var user = await _user.GetById(Guid.Parse(id));
            user.Contacts = await _contact.GetContactByUserId(user.Id);
            return Json((UserComplexModel)user, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        [HttpPost]
        [AllowAnonymous]
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
                    case "DbUpdateException": return StatusCode(409,"Error This user name already exist.");
                }
            }
            
            return StatusCode(200);
        }

        [HttpPost("AddContact")]
        [Authorize(Roles= "User,UltraAdmin")]
        public async Task<IActionResult> AddContact([FromBody] ContactModel model)
        {
            return Json(await _contact.AddContacts(User.Identity.Name, model));
        }

        [HttpPost("UpdateContact")]
        [Authorize(Roles = "User,UltraAdmin")]
        public async Task<IActionResult> UpdateContact([FromBody] ContactModel model)
        {
            await _contact.Update(model);
            return StatusCode(200);
        }
        [HttpDelete("DeleteContact/{id}")]
        [Authorize(Roles = "User,UltraAdmin")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            try
            {
                await _contact.Delete(id);
                return StatusCode(204);
            }
            catch
            {
                return StatusCode(500);
            }
            
        }

        [HttpPut]
        [Authorize(Roles = "User,UltraAdmin")]
        public async Task<JsonResult> Update(User model)
        {
            await _user.Update(model);
            return Json(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User,UltraAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _user.Delete(id);
            return StatusCode(202,"Deleted");
        }
    }
}