using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MAuthen.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class TestController : Controller
    {
        [HttpPost]
        [Authorize(Roles = "Tester")]

        public IActionResult Test()
        {
            var test = User;
            return Json("Eiii");
        }
    }
}