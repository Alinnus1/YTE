using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YTE.Code.Base;
using YTE.Models;

namespace YTE.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ControllerDependencies dependencies)
           : base(dependencies)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
