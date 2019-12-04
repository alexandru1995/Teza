using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestIntegrationApplication.Controllers
{
    public class TestController : Controller
    {
        [Authorize]
        public IActionResult Authentication()
        {
            return Json("Somple Authentication");
        }

        [Authorize]
        public IActionResult User()
        {
            return View();
        }
    }
}