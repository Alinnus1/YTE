﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using YTE.BusinessLogic.Implementation.Role;
using YTE.Code.Base;

namespace YTE.WebApp.Controllers
{
    public class RoleController : BaseController
    {
        private readonly RoleService Service;

        //authorize by admin.
        public RoleController(ControllerDependencies dependencies, RoleService roleService) : base(dependencies)
        {
            this.Service = roleService;
        }

        [HttpGet]
        public JsonResult Show()
        {
            var rolesList = Service.GetRoles();
            var rolesListG = new SelectList(rolesList, "Value", "Text");

            return Json(rolesListG);
        }

        [HttpGet]
        public JsonResult Specific(Guid id)
        {
            var rolesList = Service.GetRolesOfUser(id);
            var rolesListG = new SelectList(rolesList, "Value", "Text");

            return Json(rolesListG);
        }
    }
}
