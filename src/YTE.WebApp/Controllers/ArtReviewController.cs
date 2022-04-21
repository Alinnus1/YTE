using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.ArtReview.Model;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{

    public class ArtReviewController : BaseController
    {
        private readonly ArtReviewService Service;

        public ArtReviewController(ControllerDependencies dependencies, ArtReviewService service) : base(dependencies)
        {
            this.Service = service;
        }

        [HttpGet]
        public IActionResult ListReviewsOfArt(Guid id, string sortOrder, int pageNumber = 1)
        {
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var model = Service.GetReviewsOfArt(id, "Score", true, pageNumber);

            switch (sortOrder)
            {
                case "score_asc":
                    model = Service.GetReviewsOfArt(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = Service.GetReviewsOfArt(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = Service.GetReviewsOfArt(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }

        #region Specific User
        [HttpGet]
        public IActionResult ListReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var model = Service.GetSpecificUserReviewsOfArt(id, "Score", true, pageNumber);
            ViewBag.UserName = model.FirstOrDefault()?.UserName;
            switch (sortOrder)
            {
                case "score_asc":
                    model = Service.GetSpecificUserReviewsOfArt(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = Service.GetSpecificUserReviewsOfArt(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = Service.GetSpecificUserReviewsOfArt(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListFilmReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var model = Service.GetSpecificUserReviewsOfFilms(id, "Score", true, pageNumber);
            ViewBag.UserName = model.FirstOrDefault()?.UserName;
            switch (sortOrder)
            {
                case "score_asc":
                    model = Service.GetSpecificUserReviewsOfFilms(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = Service.GetSpecificUserReviewsOfFilms(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = Service.GetSpecificUserReviewsOfFilms(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListMangaReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var model = Service.GetSpecificUserReviewsOfMangas(id, "Score", true, pageNumber);
            ViewBag.UserName = model.FirstOrDefault()?.UserName;
            switch (sortOrder)
            {
                case "score_asc":
                    model = Service.GetSpecificUserReviewsOfMangas(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = Service.GetSpecificUserReviewsOfMangas(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = Service.GetSpecificUserReviewsOfMangas(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListGameReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var model = Service.GetSpecificUserReviewsOfGames(id, "Score", true, pageNumber);
            ViewBag.UserName = model.FirstOrDefault()?.UserName;
            switch (sortOrder)
            {
                case "score_asc":
                    model = Service.GetSpecificUserReviewsOfGames(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = Service.GetSpecificUserReviewsOfGames(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = Service.GetSpecificUserReviewsOfGames(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }


        #endregion

        [HttpGet]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Create(Guid id)
        {
            var model = new CreateArtReviewModel();
            model.ArtObjectId = id;
            if (!Service.Check(id, CurrentUser.UserName))
            {
                return View(model);
            }
            else
            {
                string urlAnterior = Request.Headers["Referer"].ToString();
                return Redirect(urlAnterior);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Create(CreateArtReviewModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }
            Service.CreateArtReview(model);

            return RedirectToAction("ListReviewsOfArt", "ArtReview", new { id = model.ArtObjectId });
        }


        [HttpGet]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Edit(Guid id, string type)
        {
            var model = Service.EditArtReview(id, type);
            ViewBag.Name = model.ArtName;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Edit(Guid id, string type, EditArtReviewModel model)
        {
            if (model == null)
            {
                return View("Error_");
            }
            if (Service.UpdateArtReviewOfCurrent(model, id, type))
            {
                return RedirectToAction("ListReviewsOfArt", "ArtReview", new { id = id });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Some error has occured!");
                return View(model);
            }

        }


        [HttpGet]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Delete(Guid id, string type)
        {
            if (Service.DeleteArtReviewOfCurrent(id, type))
            {
                string urlAnterior = Request.Headers["Referer"].ToString();
                return Redirect(urlAnterior);

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Your operation could not be completed!");
                string urlAnterior = Request.Headers["Referer"].ToString();
                return Redirect(urlAnterior);
            }

        }

        [HttpGet]
        public bool Check(Guid id, string type)
        {
            return Service.Check(id, type);
        }
    }
}
