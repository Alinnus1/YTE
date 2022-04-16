using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using YTE.BusinessLogic.Implementation.Account;
using YTE.Code.Base;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.Account.Model;
using YTE.Common.DTOS;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.AspNetCore.Authorization;

namespace YTE.WebApp.Controllers
{

    public class UserAccountController : BaseController
    {
        private readonly UserAccountService Service;

        public UserAccountController(ControllerDependencies dependencies, UserAccountService service)
           : base(dependencies)
        {
            this.Service = service;
        }

        [HttpGet]

        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Details()
        {
            var model = Service.DetailsUserAccount();

            return View(model);
        }

        [HttpGet]

        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Edit()
        {
            var model = Service.EditUserAccount();

            return View(model);
        }

        [HttpPost]

        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Edit(EditUserAccountModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }

            if (Service.UpdateUserAccount(model))
            {
                return RedirectToAction("Details", "UserAccount");

            }
            ModelState.AddModelError(string.Empty, "Password does not match the current password!");
            return View(model);

        }

        [HttpGet]

        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult Delete()
        {
            var model = new DeleteUserAccountModel();

            return View(model);
        }

        [HttpPost]

        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public async Task<IActionResult> Delete(DeleteUserAccountModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }
            if (Service.DeleteUserAccount(model))
            {
                await Logout();
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Password does not match the current password!");
            return View(model);

        }

        [HttpGet]

        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult ChangePassword()
        {
            var model = new ChangePassModel();

            return View(model);
        }

        [HttpPost]

        [Authorize(Roles = "Admin,User,ModManga,ModFilm,ModVideoGame")]
        public IActionResult ChangePassword(ChangePassModel model)
        {
            if (model == null)
            {
                return View("Error_NotFound");
            }

            if (Service.ChangePassword(model))
            {
                return RedirectToAction("Details", "UserAccount");
            }

            ModelState.AddModelError(string.Empty, "Old password does not match the current password!");
            return View(model);

        }

        [HttpGet]
        public FileContentResult Picture(Guid id)
        {
            var image = Service.ProfilePicture(id);

            return File(image, "image/jpeg", "user_profile_picture");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (CurrentUser.IsAuthenticated)
            {
                return View("Error_Unauthorized");
            }
            var model = new RegisterModel();
            return View("Register", model);
        }

    
        [HttpGet]
        public IActionResult MailSent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {

            if (model == null)
            {
                return View("Error_NotFound");
            }

            Service.RegisterNewUser(model);

            
            return RedirectToAction("MailSent", "UserAccount");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = Service.Login(model.Email, model.Password);

            if (!user.IsAuthenticated)
            {
                model.AreCredentialsInvalid = true;
                return View(model);
            } else if (!user.EmailConfirmed)
            {
                model.EmailUnConfirmed = true;
                return View(model);
            } else
            {

                await LogIn(user);
            }
            

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await LogOut();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult DemoPage()
        {
            var model = Service.GetUsers();

            return View(model);
        }

        private async Task LogIn(CurrentUserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.UserName}"),
                new Claim(ClaimTypes.Email,  user.Email)
            };

            user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                    scheme: "YTECookies",
                    principal: principal);
        }

        private async Task LogOut()
        {
            await HttpContext.SignOutAsync(scheme: "YTECookies");
        }

        [HttpGet]
        public IActionResult ConfirmationEmail(Guid id, string type)
        {

            var model = Service.ConfirmEmail(id, type);
            return View(model);

        }

        [HttpGet]
        public IActionResult ResendConfirmation(string id)
        {
            var model = Service.ResendConfirmation(id);
            return View(model);

        }

        [HttpGet]
        public IActionResult ResetPassConfirmation(string id)
        {
            var model = Service.ResetPassEmail(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(Guid id, string type)
        {
            var model = Service.ResetPasswordForm(id, type);
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(Guid id, string type,ForgotPassModel model)
        {
            Service.ResetPassword(model);
            return RedirectToAction("Login","UserAccount");
        }
    }
}