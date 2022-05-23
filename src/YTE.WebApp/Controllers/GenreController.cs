using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Genre.Model;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers.GenreControllers
{
    [Authorize(Roles = "Admin,ModManga,ModFilm,ModVideoGame")]
    public class GenreController : BaseController
    {
        private readonly GenreService Service;
        public GenreController(ControllerDependencies dependencies, GenreService service) : base(dependencies)
        {
            this.Service = service;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateGenreModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateGenreModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }
            Service.CreateNewGenre(model);

            return RedirectToAction("List", "Genre");
        }

        [HttpGet]
        public IActionResult List(int id)
        {
            var model = new List<ListGenreModel>();
            if (id == 0)
            {
                model = Service.GetGenres();
            }
            else
            {
                model = Service.GetGenresSpecific(id);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id, int type)
        {
            Service.DeleteGenre(id, type);

            return RedirectToAction("List", "Genre");
        }

        [HttpGet]
        public JsonResult Show()
        {
            var genreList = Service.GetGenreTypes();
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }

        #region FilmGenre
        [HttpGet]
        public JsonResult ShowFilmGenres()
        {
            var genreList = Service.GetFilmGenres();
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }

        [HttpGet]
        public JsonResult ShowSpecificFilmGenres(Guid id)
        {
            var genreList = Service.GetFilmGenresOfFilmS(id);
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }
        #endregion
        #region MangaGenre
        [HttpGet]
        public JsonResult ShowMangaGenres()
        {
            var genreList = Service.GetMangaGenres();
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }

        [HttpGet]
        public JsonResult ShowSpecificMangaGenres(Guid id)
        {
            var genreList = Service.GetMangaGenresOfMangaS(id);
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }
        #endregion
        #region VideoGame
        [HttpGet]
        public JsonResult ShowVideoGameGenres()
        {
            var genreList = Service.GetVideoGameGenres();
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }

        [HttpGet]
        public JsonResult ShowSpecificVideoGameGenres(Guid id)
        {
            var genreList = Service.GetVideoGameGenresOfGameS(id);
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }
        #endregion
        #region Book
        [HttpGet]
        public JsonResult ShowBookGenres()
        {
            var genreList = Service.GetBookGenres();
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }

        [HttpGet]
        public JsonResult ShowSpecificBookGenres(Guid id)
        {
            var genreList = Service.GetBookGenresOfBookS(id);
            var genreListG = new SelectList(genreList, "Value", "Text");

            return Json(genreListG);
        }
        #endregion
    }
}
