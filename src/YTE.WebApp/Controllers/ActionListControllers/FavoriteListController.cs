using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using YTE.BusinessLogic.Implementation.FavoriteList;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
    public class FavoriteListController : BaseController
    {
        private readonly FavoriteListService Service;
        public FavoriteListController(ControllerDependencies dependencies, FavoriteListService favoriteListService) : base(dependencies)
        {
            this.Service = favoriteListService;
        }

        [HttpGet]
        public IActionResult List(string id, int pageNumber = 1)
        {
            var model = Service.GetOf(id, pageNumber);
            return View(model);
        }

        [HttpPost]
        public void Add(Guid id)
        {
            Service.Add(CurrentUser.Id, id);
        }

        [HttpPost]
        public void Remove(Guid id)
        {
            Service.Remove(id);
        }
    }
}
