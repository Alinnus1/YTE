﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using YTE.BusinessLogic.Implementation.FollowList;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers.ActionListControllers
{
    [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
    public class FollowListController : BaseController
    {
        private readonly FollowListService Service;
        public FollowListController(ControllerDependencies dependencies, FollowListService followListService) : base(dependencies)
        {
            this.Service = followListService;
        }

        [HttpGet]
        public IActionResult Of(string id, string currentFilter, string searchString, int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            ViewData["UserName"] = Service.GetFollowerUserName(id);

            var model = Service.GetUserFollowings(id, searchString, pageNumber);

            return View(model);
        }

        [HttpGet]
        public IActionResult FollowersOf(string id, string currentFilter, string searchString, int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            ViewData["UserName"] = Service.GetFollowerUserName(id);

            var model = Service.GetFollowersOf(id, searchString, pageNumber);

            return View(model);
        }

        [HttpPost]
        public void Add(Guid id)
        {
            Service.Add(id);
        }

        [HttpPost]
        public void Remove(Guid id)
        {
            Service.Remove(id);
        }
    }
}
