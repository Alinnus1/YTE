using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using YTE.BusinessLogic.Implementation.Film.Model;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Manga;
using YTE.BusinessLogic.Implementation.Manga.Model;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    
    public class MangaController : BaseController
    {
        private readonly MangaService Service;
        private readonly GenreService GenreService;

        public MangaController(ControllerDependencies dependencies, MangaService service, GenreService genreService) : base(dependencies)
        {
            this.Service = service;
            this.GenreService = genreService;
        }

        [Authorize(Roles = "ModManga,Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateMangaModel();

            return View("Create", model);
        }
        [Authorize(Roles = "ModManga,Admin")]
        [HttpPost]
        public IActionResult Create(CreateMangaModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");

            }
            
            Service.CreateNewManga(model);

            return RedirectToAction("List", "Manga");
        }

        [HttpGet]
        public IActionResult List(string currentFilter,string searchString,int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var model = Service.GetMangasFilter( searchString, pageNumber);

            return View(model);
        }
        
        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var model = Service.DetailsManga(id);
            model.GenreList = GenreService.GetMangaGenresOfManga(id);
            ViewBag.Name = model.Name;
            return View(model);
        }

        [Authorize(Roles = "ModManga,Admin")]
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var model = Service.EditManga(id);
            return View(model);
        }

        [Authorize(Roles = "ModManga,Admin")]
        [HttpPost]
        public IActionResult Edit(Guid id, EditMangaModel input)
        {
            if (input == null)
            {
                return View("Error_NotFound");

            }
            Service.UpdateManga( input);

            return RedirectToAction("Details", "Manga", new
            {
                id = id
            });
        }

        [Authorize(Roles = "ModManga,Admin")]
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Service.DeleteManga(id);

            return RedirectToAction("List", "Manga");
        }


        [HttpGet]
        public JsonResult Attributes()
        {
            var attributesList = Service.GetMangaAttributes();

            return Json(attributesList);
        }


    }
}
