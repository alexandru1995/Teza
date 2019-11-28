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

namespace TestIntegrationApplication.Controllers
{
    public class HomeController : Controller
    {
        private string _SESSION_KEY = "sub";
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
    }
}
