using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.ArtReview.Model;
using YTE.BusinessLogic.Implementation.UserProfile;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{

    public class ArtReviewController : BaseController
    {
        private readonly ArtReviewService ArtReviewService;
        private readonly UserProfileService UserProfileService;

        public ArtReviewController(ControllerDependencies dependencies, ArtReviewService artReviewService, UserProfileService userProfileService) : base(dependencies)
        {
            this.ArtReviewService = artReviewService;
            this.UserProfileService = userProfileService;
        }

        [HttpGet]
        public IActionResult ListReviewsOfArt(Guid id, string sortOrder, int pageNumber = 1)
        {
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var model = ArtReviewService.GetReviewsOfArt(id, "Score", true, pageNumber);

            switch (sortOrder)
            {
                case "score_asc":
                    model = ArtReviewService.GetReviewsOfArt(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = ArtReviewService.GetReviewsOfArt(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = ArtReviewService.GetReviewsOfArt(id, "Date", true, pageNumber);
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
            if (!UserProfileService.IsUserNameValid(id))
            {
                return View("Error_NotFound");
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.UserName = id;
            var model = ArtReviewService.GetSpecificUserReviewsOfArt(id, "Score", true, pageNumber);

            switch (sortOrder)
            {
                case "score_asc":
                    model = ArtReviewService.GetSpecificUserReviewsOfArt(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = ArtReviewService.GetSpecificUserReviewsOfArt(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = ArtReviewService.GetSpecificUserReviewsOfArt(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListFilmReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            if (!UserProfileService.IsUserNameValid(id))
            {
                return View("Error_NotFound");
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.UserName = id;
            var model = ArtReviewService.GetSpecificUserReviewsOfFilms(id, "Score", true, pageNumber);

            switch (sortOrder)
            {
                case "score_asc":
                    model = ArtReviewService.GetSpecificUserReviewsOfFilms(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = ArtReviewService.GetSpecificUserReviewsOfFilms(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = ArtReviewService.GetSpecificUserReviewsOfFilms(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListMangaReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            if (!UserProfileService.IsUserNameValid(id))
            {
                return View("Error_NotFound");
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.UserName = id;
            var model = ArtReviewService.GetSpecificUserReviewsOfMangas(id, "Score", true, pageNumber);

            switch (sortOrder)
            {
                case "score_asc":
                    model = ArtReviewService.GetSpecificUserReviewsOfMangas(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = ArtReviewService.GetSpecificUserReviewsOfMangas(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = ArtReviewService.GetSpecificUserReviewsOfMangas(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListGameReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            if (!UserProfileService.IsUserNameValid(id))
            {
                return View("Error_NotFound");
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.UserName = id;
            var model = ArtReviewService.GetSpecificUserReviewsOfGames(id, "Score", true, pageNumber);

            switch (sortOrder)
            {
                case "score_asc":
                    model = ArtReviewService.GetSpecificUserReviewsOfGames(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = ArtReviewService.GetSpecificUserReviewsOfGames(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = ArtReviewService.GetSpecificUserReviewsOfGames(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListBookReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            if (!UserProfileService.IsUserNameValid(id))
            {
                return View("Error_NotFound");
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.UserName = id;
            var model = ArtReviewService.GetSpecificUserReviewsOfBooks(id, "Score", true, pageNumber);

            switch (sortOrder)
            {
                case "score_asc":
                    model = ArtReviewService.GetSpecificUserReviewsOfBooks(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = ArtReviewService.GetSpecificUserReviewsOfBooks(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = ArtReviewService.GetSpecificUserReviewsOfBooks(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListAlbumReviewsOf(string id, string sortOrder, int pageNumber = 1)
        {
            if (!UserProfileService.IsUserNameValid(id))
            {
                return View("Error_NotFound");
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScoreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "score_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            ViewBag.UserName = id;
            var model = ArtReviewService.GetSpecificUserReviewsOfAlbums(id, "Score", true, pageNumber);

            switch (sortOrder)
            {
                case "score_asc":
                    model = ArtReviewService.GetSpecificUserReviewsOfAlbums(id, "Score", false, pageNumber);
                    break;
                case "Date":
                    model = ArtReviewService.GetSpecificUserReviewsOfAlbums(id, "Date", false, pageNumber);
                    break;
                case "date_desc":
                    model = ArtReviewService.GetSpecificUserReviewsOfAlbums(id, "Date", true, pageNumber);
                    break;
                default:

                    break;
            }

            return View(model);
        }


        #endregion

        [HttpGet]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame,ModBook,ModAlbum")]
        public IActionResult Create(Guid id)
        {
            var model = new CreateArtReviewModel();
            model.ArtObjectId = id;
            model.ExperiencedAt = DateTime.Now;
            if (!ArtReviewService.Check(id, CurrentUser.UserName))
            {
                return View(model);
            }
            else
            {
                var previousUrl = Request.Headers["Referer"].ToString();

                return Redirect(previousUrl);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame,ModBook,ModAlbum")]
        public IActionResult Create(CreateArtReviewModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }
            ArtReviewService.CreateArtReview(model);

            return RedirectToAction("ListReviewsOfArt", "ArtReview", new { id = model.ArtObjectId });
        }


        [HttpGet]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame,ModBook,ModAlbum")]
        public IActionResult Edit(Guid id, string type)
        {
            var model = ArtReviewService.EditArtReview(id, type);

            ViewBag.Name = model.ArtName;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame,ModBook,ModAlbum")]
        public IActionResult Edit(Guid id, string type, EditArtReviewModel model)
        {
            if (model == null)
            {
                return View("Error_");
            }
            if (ArtReviewService.UpdateArtReviewOfCurrent(model, id, type))
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
        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame,ModBook,ModAlbum")]
        public IActionResult Delete(Guid id, string type)
        {
            var previousUrl = string.Empty;

            if (ArtReviewService.DeleteArtReviewOfCurrent(id, type))
            {
                previousUrl = Request.Headers["Referer"].ToString();

                return Redirect(previousUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Your operation could not be completed!");
                previousUrl = Request.Headers["Referer"].ToString();

                return Redirect(previousUrl);
            }
        }

        [HttpGet]
        public bool Check(Guid id, string type)
        {
            return ArtReviewService.Check(id, type);
        }
    }
}
