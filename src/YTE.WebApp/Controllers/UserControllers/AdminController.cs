using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using YTE.BusinessLogic.Implementation.Admin;
using YTE.BusinessLogic.Implementation.Admin.Model;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {

        private readonly AdminService Service;
        public AdminController(ControllerDependencies dependencies, AdminService adminService)
            : base(dependencies)
        {
            this.Service = adminService;
        }

        [HttpGet]
        public IActionResult PrivateAction()
        {
            return Ok("top secret");
        }


        [HttpGet]
        public IActionResult UserList(string currentFilter, string searchString, int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var model = Service.GetUsers(searchString, pageNumber);
            return View(model);
        }

        [HttpGet]
        public IActionResult UserDetails(Guid id)
        {
            var model = Service.GetUser(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult EditUser(Guid id)
        {
            var model = Service.EditUser(id);
            return View(model);

        }

        [HttpPost]
        public IActionResult EditUser(EditUserModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }

            Service.UpdateUser(model);
            return RedirectToAction("UserDetails", "Admin", new { id = model.Id });
        }

        [HttpGet]
        public IActionResult DeleteUser(Guid id)
        {
            Service.DeleteUser(id);

            return RedirectToAction("UserList", "Admin");
        }
    }
}
