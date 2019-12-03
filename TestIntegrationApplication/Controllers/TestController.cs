using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestIntegrationApplication.Controllers
{
    public class TestController : Controller
    {
        [Authorize]
        public IActionResult Authentication()
        {
            return Json("Uraaaaaaa");
        }
    }
}