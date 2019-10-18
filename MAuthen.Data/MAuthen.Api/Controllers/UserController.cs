using MAuthen.Domain.Models;
using MAuthen.Domain.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _user;
        public UserController(IUserRepository user)
        {
            _user = user;
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
        public async Task<IActionResult> Add([FromBody]User model)
        {
            try
            {
                await _user.Create(model);
            }
            catch(Exception err)
            {
                switch (err.GetType().Name)
                {
                    case "DbUpdateException": return Json("Error This email alredy exist.");
                }
            }
            
            return StatusCode(201, "Successful creation");
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