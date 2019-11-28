using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TestIntegrationApplication.Helpers;
using TestIntegrationApplication.Models;
using Microsoft.Extensions.Primitives;

namespace TestIntegrationApplication.Controllers
{
    public class HomeController : Controller
    {
        private IJwtToken _token;
        public HomeController(IJwtToken token)
        {
            _token = token;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task <IActionResult> Privacy()
        {
            ViewData["clientID"] =  _token.Create($"{this.Request.Host}");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("OnLogin")]
        public async Task<IActionResult> OnLogin()
        {
            StringValues token;
            HttpContext.Request.Form.TryGetValue("Token", out token);
            var test = token;
            return null;
        }
    }
}
