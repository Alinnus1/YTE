using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using YTE.BusinessLogic.Implementation.Album;
using YTE.BusinessLogic.Implementation.Album.Model;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    public class AlbumController : BaseController
    {
        private readonly AlbumService Service;
        private readonly GenreService GenreService;

        public AlbumController(ControllerDependencies dependencies, AlbumService service, GenreService genreService) : base(dependencies)
        {
            this.Service = service;
            this.GenreService = genreService;
        }

        [HttpGet]
        [Authorize(Roles = "ModAlbum,Admin")]
        public IActionResult Create()
        {
            var model = new CreateAlbumModel();
            model.ReleaseDate = DateTime.Now.AddYears(-10);

            return View("Create", model);
        }

        [HttpPost]
        [Authorize(Roles = "ModAlbum,Admin")]
        public IActionResult Create(CreateAlbumModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }
            Service.CreateNewAlbum(model);

            return RedirectToAction("List", "Album");
        }

        [HttpGet]
        public IActionResult List(string currentFilter, string searchString, int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var model = Service.GetAlbumsFilter(searchString, pageNumber);

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var model = Service.DetailsAlbum(id);
            model.GenreList = GenreService.GetAlbumGenresOfAlbum(id);

            ViewBag.Name = model.Name;

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "ModAlbum,Admin")]
        public IActionResult Edit(Guid id)
        {
            var model = Service.EditAlbum(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "ModFilm,Admin")]
        public IActionResult Edit(Guid id, EditAlbumModel input)
        {
            if (input == null)
            {
                return View("Error_NotFound");

            }
            Service.UpdateAlbum(input);

            return RedirectToAction("Details", "Album", new
            {
                id = id
            });
        }

        [HttpGet]
        [Authorize(Roles = "ModAlbum,Admin")]
        public IActionResult Delete(Guid id)
        {
            Service.DeleteAlbum(id);

            return RedirectToAction("List", "Album");
        }


        [HttpGet]
        public JsonResult Attributes()
        {
            var attributesList = Service.GetAlbumAttributes();

            return Json(attributesList);
        }
    }
}
