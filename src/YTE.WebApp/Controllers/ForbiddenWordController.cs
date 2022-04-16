using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.ForbiddenWord;
using YTE.BusinessLogic.Implementation.ForbiddenWord.Model;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ForbiddenWordController : BaseController
    {
        private readonly ForbiddenWordService Service;
        public ForbiddenWordController(ControllerDependencies dependencies,ForbiddenWordService forbiddenWordService) : base(dependencies)
        {
            this.Service = forbiddenWordService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateForbiddenWordModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateForbiddenWordModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }
            Service.CreateNewForbiddenWord(model);
            return RedirectToAction("List", "ForbiddenWord");
        }

        [HttpGet]
        public IActionResult List()
        {
            var model = Service.GetForbiddenWords();
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Service.DeleteForbiddenWord(id);
            return RedirectToAction("List", "ForbiddenWord");
        }
    }
}
