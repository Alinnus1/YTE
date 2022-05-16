using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using YTE.BusinessLogic.Implementation.UserProfile;
using YTE.BusinessLogic.Implementation.UserProfile.Model;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    public class UserProfileController : BaseController
    {
        private readonly UserProfileService Service;
        public UserProfileController(ControllerDependencies dependencies, UserProfileService userProfile) : base(dependencies)
        {
            this.Service = userProfile;
        }

        [HttpGet]
        public IActionResult List(string currentFilter, string searchString, int pageNumber = 1)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var model = new List<ListUserProfileModel>();
            if (!String.IsNullOrEmpty(searchString))
            {
                model = Service.GetUserProfilesFilter(searchString, pageNumber);
            }
            else
            {
                model = Service.GetUserProfiles(pageNumber);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var model = Service.GetUserProfile(id);
            return View(model);
        }
    }
}
