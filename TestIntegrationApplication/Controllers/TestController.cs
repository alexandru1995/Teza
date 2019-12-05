using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestIntegrationApplication.Controllers
{
    public class TestController : Controller
    {
        [Authorize]
        public IActionResult User()
        {
            return Json("Simple Authentication");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Administrator()
        {
            return Json("Administrator");
        }
    }
}