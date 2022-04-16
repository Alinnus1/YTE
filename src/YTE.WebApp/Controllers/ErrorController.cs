using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
