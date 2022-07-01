using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.Account;
using YTE.BusinessLogic.Implementation.Admin.Model;
using YTE.BusinessLogic.Implementation.Admin.Validation;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.Role;
using YTE.Common.DTOS;
using YTE.Common.Exceptions;
using YTE.Common.Extensions;
using YTE.DataAccess;
using YTE.Entities;

namespace YTE.BusinessLogic.Implementation.Admin
{
    public class AdminService : BaseService
    {
        private readonly ImageService ImageService;
        private readonly RoleService RoleService;
        private readonly EditUserValidator EditUserValidator;
        public AdminService(ServiceDependencies dependencies, ImageService imageService, RoleService roleService) : base(dependencies)
        {
            this.RoleService = roleService;
            this.ImageService = imageService;
            this.EditUserValidator = new EditUserValidator(UnitOfWork);
        }

        public List<ListUserModel> GetUsers(string searchString, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "";
            }

            var users = UnitOfWork.Users.Get()
                .Include(a => a.Image)
                .Where(a => a.UserName.Contains(searchString))
                .Select(a => Mapper.Map<User, ListUserModel>(a));
            var paginatedUsers = PaginatedList<ListUserModel>.Create(users, pageNumber, 25);

            return paginatedUsers;
        }

        public DetailsUserModel GetUser(Guid id)
        {
            var user = UnitOfWork.Users.Get()
                .Include(u => u.UserRoles)
                    .ThenInclude(u => u.Role)
                .Include(a => a.Gender)
                .Include(a => a.Image)
                .Where(a => a.Id == id)
                .Select(a => Mapper.Map<User, DetailsUserModel>(a))
                .FirstOrDefault();

            if (user == null)
            {
                throw new NotFoundErrorException("User Not Found");
            }

            return user;
        }

        public EditUserModel EditUser(Guid id)
        {
            return UnitOfWork.Users.Get()
                .Include(u => u.Image)
                .Include(u => u.Gender)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.Id == id)
                .Select(u => Mapper.Map<User, EditUserModel>(u))
                .FirstOrDefault();
        }

        public void UpdateUser(EditUserModel model)
        {
            ExecuteInTransaction(uow =>
            {
                var user = uow.Users.Get()
                    .Include(u => u.Image)
                    .FirstOrDefault(u => u.Id == model.Id);
                if (user == null)
                {
                    return;
                }

                EditUserValidator.Validate(model).ThenThrow(model);
                user.UserName = model.UserName;
                user.Name = model.LastName;
                user.Pronoun = model.FirstName;
                user.GenderId = model.GenderId;
                user.Age = model.Age;
                user.Image.IsActive = false;
                if (model.NewPassword != null)
                {
                    user.PasswordHash = UserAccountService.ComputeSha256Hash(model.NewPassword);
                }

                ImageService.SetImage(model, uow, user);
                RoleService.SetRoles(model, uow, user);

                uow.Users.Update(user);
                uow.SaveChanges();
            });
        }

        public void DeleteUser(Guid id)
        {
            ExecuteInTransaction(uow =>
            {
                var user = uow.Users.Get()
                    .FirstOrDefault(u => u.Id == id);

                uow.Users.Delete(user);
                uow.SaveChanges();
            });
        }
    }
}
