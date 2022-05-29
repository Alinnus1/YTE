using Microsoft.AspNetCore.Mvc;
using YTE.BusinessLogic.Implementation.Book;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.Code.Base;
using Microsoft.AspNetCore.Authorization;
using YTE.BusinessLogic.Implementation.Book.Model;
using System;

namespace YTE.WebApp.Controllers.ArtControllers
{
    public class BookController : BaseController
    {
        private readonly BookService Service;
        private readonly GenreService GenreService;

        public BookController(ControllerDependencies dependencies, BookService service, GenreService genreService) : base(dependencies)
        {
            this.Service = service;
            this.GenreService = genreService;
        }

        [HttpGet]
        [Authorize(Roles = "ModBook,Admin")]
        public IActionResult Create()
        {
            var model = new CreateBookModel();
            model.ReleaseDate = DateTime.Now.AddYears(-10);

            return View("Create", model);
        }

        [HttpPost]
        [Authorize(Roles = "ModBook,Admin")]
        public IActionResult Create(CreateBookModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }
            Service.CreateNewBook(model);

            return RedirectToAction("List","Book");
        }

        [HttpGet]
        public IActionResult List(string currentFilter, string searchString, int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var model = Service.GetBooksFilter(searchString, pageNumber);

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var model = Service.DetailsBook(id);
            model.GenreList = GenreService.GetBookGenresOfBook(id);

            ViewBag.Name = model.Name;

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "ModBook,Admin")]
        public IActionResult Edit(Guid id)
        {
            var model = Service.EditBook(id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "ModBook,Admin")]
        public IActionResult Edit(Guid id, EditBookModel input)
        {
            if (input == null)
            {
                return View("Error_NotFound");

            }
            Service.UpdateBook(input);

            return RedirectToAction("Details", "Book", new
            {
                id = id
            });
        }

        [HttpGet]
        [Authorize(Roles = "ModBook,Admin")]
        public IActionResult Delete(Guid id)
        {
            Service.DeleteBook(id);

            return RedirectToAction("List", "Book");
        }


        [HttpGet]
        public JsonResult Attributes()
        {
            var attributesList = Service.GetBookAttributes();

            return Json(attributesList);
        }
    }
}
