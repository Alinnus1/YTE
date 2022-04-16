using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.Admin.Model;
using YTE.Common.DTOS;
using YTE.DataAccess;
using YTE.Entities;

namespace YTE.BusinessLogic.Implementation.Role
{
    public class RoleService : BaseService
    {
        public RoleService(ServiceDependencies dependencies) : base(dependencies)
        {
        }

        public List<ListItem<string, int>> GetRoles()
        {
            return UnitOfWork.Roles.Get()
                .Where(r=> r.Name != "User")
                .Select(r => new ListItem<string, int>
                {
                    Text = $"{r.Name}",
                    Value = r.Id
                })
                .ToList();
        }

        public List<ListItem<string, int>> GetRolesOfUser(Guid id)
        {

            return  UnitOfWork.Users.Get()
                .Where(u => u.Id == id)
                .Select(u => u.UserRoles.Select(ur => ur.Role).Where(r => r.Name != "User").Select(r => new ListItem<string, int>
                {
                    Text = $"{r.Name}",
                    Value = r.Id
                }).ToList()).Single();
        }

        public void SetRoles(EditUserModel model, UnitOfWork uow, User user)
        {
            var roles = uow.UserRoles.Get()
                                .Where(ur => ur.UserId == user.Id);
            var currentRoleIds = uow.UserRoles.Get()
                .Where(ur => ur.UserId == user.Id && ur.Role.Name != "User")
                .Select(ur => ur.RoleId)
                .ToList();

            if (model.selectedRoles != null)
            {
                var upComingRoleIds = model.selectedRoles;
                var common = currentRoleIds.Intersect(upComingRoleIds).ToList();
                currentRoleIds.RemoveAll(x => common.Contains(x));
                upComingRoleIds.RemoveAll(x => common.Contains(x));
                DeleteUserRoleR(uow, user, currentRoleIds);
                foreach (var roleId in upComingRoleIds)
                {
                    CreateUserRoleR(uow, user, roleId);
                }
            }
            else
            {
                DeleteUserRoleR(uow, user, currentRoleIds);
            }
        }

        public void DeleteUserRoleR(UnitOfWork uow, User user, List<int> currentRoleIds)
        {
            foreach (var roleId in currentRoleIds)
            {
                var relationsUserRole = uow.UserRoles.Get()
                    .FirstOrDefault(ur => ur.RoleId == roleId && ur.UserId == user.Id);
                uow.UserRoles.Delete(relationsUserRole);
            }
        }

        public void CreateUserRoleR(UnitOfWork uow, User user, int selectedRole)
        {
            uow.UserRoles.Insert(new UserRole()
            {
                RoleId = selectedRole,
                UserId = user.Id
            });
        }
    }
}
