using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.Account;
using YTE.BusinessLogic.Implementation.Gender;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{

    public class GenderController : BaseController
    {
        private readonly GenderService Service;
        public GenderController(ControllerDependencies dependencies,GenderService service) : base(dependencies)
        {
            this.Service = service;
        }


        [HttpGet]
        public JsonResult Show()
        {
            var genderList = Service.GetGenders();
            var genderListG = new SelectList(genderList, "Value", "Text");

            return Json(genderListG);
        }

        [HttpGet]
        public JsonResult Specific(Guid id)
        {
            var genreList = Service.GetGenderOfUser(id);
            var genreListG = new SelectList(genreList, "Value", "Text");
            return Json(genreListG);
        }
    }
}
