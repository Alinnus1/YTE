using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.ArtObject;
using YTE.BusinessLogic.Implementation.ArtObject.Model;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    public class ArtObjectController : BaseController
    {
        private readonly ArtObjectService Service;

        public ArtObjectController(ControllerDependencies dependencies, ArtObjectService service) : base(dependencies)
        {
            this.Service= service;
        }

        [HttpGet]
        public IActionResult List()
        {
            var model = Service.GetArtObjects();

            return View(model);
        }

        //[HttpGet]
        //public IActionResult Details()

        [HttpGet]
        //public IActionResult Create()
        //{
        //    var model = new CreateArtObjectModel();

        //    var artObjectTypesList = Service.GetArtObjectTypes();
        //    var artObjectTypeListG = new SelectList(artObjectTypesList, "Value", "Text");
        //    ViewBag.artObjectTypes= artObjectTypeListG;

        //    return View("Create", model);
        //}

        [HttpPost]
        public IActionResult Create(CreateArtObjectModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }

            Service.CreateNewArtObject(model);

            return RedirectToAction("Create", "ArtObject");
        }
    }
}
