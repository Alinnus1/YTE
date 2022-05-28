using Microsoft.AspNetCore.Mvc;

namespace YTE.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Error_Unauthorized()
        {
            return View();
        }
    }
}
