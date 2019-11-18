using MAuthen.Domain.Entities;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAuthen.Api.Models;
using MAuthen.Api.Services.Interfaces;
using MAuthen.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<JsonResult> Get()
        {
            var user = await _user.GetUserByUsername(User.Identity.Name);
            return Json(user);
        }

        [HttpGet("users")]
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