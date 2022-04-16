﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Manga.Model;
using YTE.BusinessLogic.Implementation.VideoGame;
using YTE.BusinessLogic.Implementation.VideoGame.Model;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    public class VideoGameController : BaseController
    {
        private readonly VideoGameService Service;
        private readonly GenreService GenreService;

        public VideoGameController(ControllerDependencies dependencies, VideoGameService service,GenreService genreService) : base(dependencies)
        {
            this.Service = service;
            this.GenreService = genreService; 
        }

        [Authorize(Roles = "ModVideoGame,Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateVideoGameModel();

            return View("Create", model);
        }
        [Authorize(Roles = "ModVideoGame,Admin")]
        [HttpPost]
        public IActionResult Create(CreateVideoGameModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");

            }
            
            Service.CreateNewVideoGame(model);

            return RedirectToAction("List", "VideoGame");
        }

        [HttpGet]
        public IActionResult List(string currentFilter,string searchString, int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var model = Service.GetVideoGamesFilter( searchString, pageNumber);

            return View(model);
        }

        
        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var model = Service.DetailsVideoGame(id);
            model.GenreList = GenreService.GetVideoGameGenresOfGame(id);
            ViewBag.Name = model.Name;
            return View(model);
        }

        [Authorize(Roles = "ModVideoGame,Admin")]
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var model = Service.EditVideoGame(id);
            return View(model);
        }

        [Authorize(Roles = "ModVideoGame,Admin")]
        [HttpPost]
        public IActionResult Edit(Guid id, EditVideoGameModel input)
        {
            if (input == null)
            {
                return View("Error_NotFound");

            }
            Service.UpdateVideoGame(input);

            return RedirectToAction("Details", "VideoGame", new
            {
                id = id
            });
        }

        [Authorize(Roles = "ModVideoGame,Admin")]
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Service.DeleteVideoGame(id);

            return RedirectToAction("List", "VideoGame");
        }

        [HttpGet]
        public JsonResult Attributes()
        {
            var attributesList = Service.GetVideoGameAttributes();

            return Json(attributesList);
        }


    }
}