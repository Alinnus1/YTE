using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.WatchList;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
    public class WatchListController : BaseController
    {
        private readonly WatchListService Service;
        public WatchListController(ControllerDependencies dependencies, WatchListService watchListService) : base(dependencies)
        {
            this.Service = watchListService;
        }

        [HttpGet]
        public IActionResult List(string id, int pageNumber = 1)
        {
            var model = Service.GetOf(id,pageNumber);
            return View(model);
        }

        
        [HttpPost]
        public void Add(Guid id)
        {
            Service.Add( id);
        }

        [HttpPost]
        public void Remove(Guid id)
        {
            Service.Remove( id);
        }
    }
}
