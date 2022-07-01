using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using YTE.BusinessLogic.Implementation.Film;
using YTE.BusinessLogic.Implementation.Film.Model;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    public class FilmController : BaseController
    {
        private readonly FilmService Service;
        private readonly GenreService GenreService;

        public FilmController(ControllerDependencies dependencies, FilmService service, GenreService genreService) : base(dependencies)
        {
            this.Service = service;
            this.GenreService = genreService;
        }

        [HttpGet]
        [Authorize(Roles = "ModFilm,Admin")]
        public IActionResult Create()
        {
            var model = new CreateFilmModel();
            model.ReleaseDate = DateTime.Now.AddYears(-10);

            return View("Create", model);
        }

        [HttpPost]
        [Authorize(Roles = "ModFilm,Admin")]
        public IActionResult Create(CreateFilmModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }
            Service.CreateNewFilm(model);

            return RedirectToAction("List", "Film");
        }

        [HttpGet]
        public IActionResult List(string currentFilter, string searchString, int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var model = Service.GetFilmsFilter(searchString, pageNumber);

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var model = Service.DetailsFilm(id);
            model.GenreList = GenreService.GetFilmGenresOfFilm(id);

            ViewBag.Name = model.Name;

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "ModFilm,Admin")]
        public IActionResult Edit(Guid id)
        {
            var model = Service.EditFilm(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "ModFilm,Admin")]
        public IActionResult Edit(Guid id, EditFilmModel input)
        {
            if (input == null)
            {
                return View("Error_NotFound");

            }
            Service.UpdateFilm(input);

            return RedirectToAction("Details", "Film", new
            {
                id = id
            });
        }

        [HttpGet]
        [Authorize(Roles = "ModFilm,Admin")]
        public IActionResult Delete(Guid id)
        {
            Service.DeleteFilm(id);

            return RedirectToAction("List", "Film");
        }


        [HttpGet]
        public JsonResult Attributes()
        {
            var attributesList = Service.GetFilmAttributes();

            return Json(attributesList);
        }
    }
}
