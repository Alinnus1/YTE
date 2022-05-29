using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.Account.Model;
using YTE.BusinessLogic.Implementation.Account.Validation;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.MailSender;
using YTE.BusinessLogic.Implementation.Token;
using YTE.Common.DTOS;
using YTE.Common.Extensions;
using YTE.DataAccess;
using YTE.Entities;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.Account
{
    public class UserAccountService : BaseService
    {
        private readonly RegisterUserAccountValidator RegisterUserValidator;
        private readonly ChangePasswordValidator ChangePasswordValidator;
        private readonly DeleteUserAccountValidator DeleteUserValidator;
        private readonly EditUserAccountValidator EditUserAccountValidator;
        private readonly ImageService ImageService;
        private readonly TokenService TokenService;
        private readonly MailSenderService MailSenderService;


        public UserAccountService(ServiceDependencies dependencies, ImageService imageService, TokenService tokenService, MailSenderService mailSenderService)
            : base(dependencies)
        {
            this.RegisterUserValidator = new RegisterUserAccountValidator(UnitOfWork);
            this.ChangePasswordValidator = new ChangePasswordValidator();
            this.DeleteUserValidator = new DeleteUserAccountValidator();

            this.EditUserAccountValidator = new EditUserAccountValidator(UnitOfWork);
            this.ImageService = imageService;
            this.TokenService = tokenService;
            this.MailSenderService = mailSenderService;
        }

        public static string ComputeSha256Hash(string rawPass)
        {
            if (rawPass is null)
            {
                return String.Empty;
            }

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawPass));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool CheckEmail(string email)
        {
            var emailBase = UnitOfWork.Users.Get()
                .FirstOrDefault(u => u.Email == email);
            return (emailBase == null) ? true : false;
        }

        public CurrentUserDto Login(string email, string password)
        {
            var passwordHash = ComputeSha256Hash(password);

            var user = UnitOfWork.Users.Get()
                .Include(u => u.Image)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);

            if (user == null)
            {
                return new CurrentUserDto { IsAuthenticated = false, EmailConfirmed = true };
            }

            if (!user.ConfirmedEmail)
            {
                return new CurrentUserDto { IsAuthenticated = true, EmailConfirmed = false };
            }

            return new CurrentUserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name,
                Pronoun = user.Pronoun,
                IsAuthenticated = true,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                EmailConfirmed = true

            };
        }

        #region WIP
        public byte[] ProfilePicture(Guid userId)
        {
            var image = UnitOfWork.Users.Get()
                .Where(u => u.Id == userId)
                .Include(u => u.Image)
                .Select(u => u.Image)
                .SingleOrDefault();

            return image.Content;
        }

        public DetailsUserAccountModel DetailsUserAccount()
        {
            var user = UnitOfWork.Users.Get()
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .Include(u => u.Image)
            .Include(u => u.Gender)
            .FirstOrDefault(u => u.UserName == CurrentUser.UserName && u.Id == CurrentUser.Id);

            return Mapper.Map<User, DetailsUserAccountModel>(user);
        }

        public EditUserAccountModel EditUserAccount()
        {
            var user = UnitOfWork.Users.Get()
                .Include(u => u.Image)
                .Include(u => u.Gender)
                .FirstOrDefault(u => u.UserName == CurrentUser.UserName && u.Id == CurrentUser.Id);

            return Mapper.Map<User, EditUserAccountModel>(user);
        }

        public bool UpdateUserAccount(EditUserAccountModel model)
        {
            return ExecuteInTransaction(uow =>
            {
                EditUserAccountValidator.Validate(model).ThenThrow(model);
                var user = uow.Users.Get()
                    .Include(u => u.Image)
                    .FirstOrDefault(u => u.Id == model.Id);

                if (model.Password == null)
                {
                    return false;
                }
                var passwordHash = ComputeSha256Hash(model.Password);

                if (user.PasswordHash == passwordHash)
                {
                    user.UserName = model.UserName;
                    user.Name = model.LastName;
                    user.Pronoun = model.FirstName;
                    user.GenderId = model.GenderId;
                    user.Age = model.Age;
                    ImageService.SetImage(model, uow, user);
                    uow.Users.Update(user);
                    uow.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public ConfirmationEmailModel ConfirmEmail(Guid id, string type)
        {
            var model = new ConfirmationEmailModel()
            {
                UserId = id,
                TokenId = type,
                IsEmailConfirmed = false,
                IsTokenExpired = false
            };
            return ExecuteInTransaction(uow =>
            {
                var user = uow.Users.Get()
                            .FirstOrDefault(u => u.Id == id);
                var token = uow.Tokens.Get()
                            .FirstOrDefault(t => t.Id == type);

                if (user != null && token != null)
                {
                    TimeSpan timeLeftToken = DateTime.Now - token.Date;
                    if (timeLeftToken.Days >= 1)
                    {
                        uow.Tokens.Delete(token);
                        uow.SaveChanges();
                        model.IsTokenExpired = true;
                        return model;
                    }
                    else
                    {
                        uow.Tokens.Delete(token);
                        user.ConfirmedEmail = true;
                        uow.Users.Update(user);
                        uow.SaveChanges();
                        model.IsEmailConfirmed = true;
                        return model;
                    }
                }
                else
                {
                    return model;
                }
            });
        }

        public void ResetPassword(ForgotPassModel model)
        {
            ExecuteInTransaction(uow =>
            {
                ChangePasswordValidator.Validate(model).ThenThrow(model);
                var user = uow.Users.Get()
                            .FirstOrDefault(u => u.Id == model.UserId);
                var token = uow.Tokens.Get()
                            .FirstOrDefault(t => t.Id == model.TokenId);
                user.PasswordHash = ComputeSha256Hash(model.NewPassword);
                uow.Tokens.Delete(token);
                uow.Users.Update(user);
                uow.SaveChanges();

            });
        }

        public ForgotPassModel ResetPasswordForm(Guid userId, string tokenId)
        {
            var model = new ForgotPassModel()
            {
                AreCredentialsValid = false
            };

            return ExecuteInTransaction(uow =>
            {
                var user = uow.Users.Get()
                            .FirstOrDefault(u => u.Id == userId);
                var token = uow.Tokens.Get()
                            .FirstOrDefault(t => t.Id == tokenId);

                if (user == null || token == null)
                {
                    return model;
                }

                TimeSpan timeLeftToken = DateTime.Now - token.Date;
                if (timeLeftToken.Days >= 1)
                {
                    uow.Tokens.Delete(token);
                    uow.SaveChanges();

                    return model;
                }
                else
                {
                    model.UserId = user.Id;
                    model.TokenId = token.Id;
                    model.AreCredentialsValid = true;

                    return model;
                }
            });
        }

        public SendForgotPassEmailModel ResetPassEmail(string email)
        {
            var model = new SendForgotPassEmailModel()
            {
                IsEmailSent = false,
                IsUserFound = false,
                Email = email,
            };

            return ExecuteInTransaction(uow =>
            {
                var user = uow.Users.Get()
                            .FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return model;
                }

                model.IsUserFound = true;
                var token = TokenService.CreateForgotPassEmailToken(uow);
                uow.SaveChanges();
                MailSenderService.SendForgotPassEmail(user.Id, user.Email, token.Id);
                model.IsEmailSent = true;
                return model;
            });
        }

        public ResendConfirmationEmailModel ResendConfirmation(string email)
        {
            var model = new ResendConfirmationEmailModel()
            {
                IsEmailSent = false,
                IsEmailConfirmed = false,
                IsUserFound = false,
                Email = email,
            };

            return ExecuteInTransaction(uow =>
            {
                var user = uow.Users.Get()
                            .FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return model;
                }

                model.IsUserFound = true;
                if (user.ConfirmedEmail)
                {
                    model.IsEmailConfirmed = true;
                    return model;
                }
                var token = TokenService.CreateConfirmationEmailToken(uow);
                uow.SaveChanges();
                MailSenderService.SendConfirmationEmail(user.Id, user.Email, token.Id);
                model.IsEmailSent = true;
                model.IsTokenCreated = true;

                return model;
            });
        }

        public bool DeleteUserAccount(DeleteUserAccountModel model)
        {
            return ExecuteInTransaction(uow =>
            {
                DeleteUserValidator.Validate(model).ThenThrow(model);
                var user = uow.Users.Get()
                    .Where(u => u.Id == CurrentUser.Id)
                    .SingleOrDefault();
                var passwordHash = ComputeSha256Hash(model.Password);

                if (user.PasswordHash != passwordHash)
                {
                    return false;
                }

                uow.Users.Delete(user);
                uow.SaveChanges();

                return true;
            });
        }

        public bool ChangePassword(ChangePassModel model)
        {
            return ExecuteInTransaction(uow =>
            {
                ChangePasswordValidator.Validate(model).ThenThrow(model);

                var userPass = uow.Users.Get()
                    .Where(u => u.Id == CurrentUser.Id)
                    .Select(u => u.PasswordHash)
                    .SingleOrDefault();
                var passwordHash = ComputeSha256Hash(model.OldPassword);

                if (userPass != passwordHash)
                {
                    return false;
                }

                var user = uow.Users.Get()
                .Where(u => u.Id == CurrentUser.Id)
                .SingleOrDefault();
                user.PasswordHash = ComputeSha256Hash(model.NewPassword);
                uow.Users.Update(user);
                uow.SaveChanges();

                return true;
            });
        }

        #endregion
        public void RegisterNewUser(RegisterModel model)
        {
            ExecuteInTransaction(uow =>
            {
                RegisterUserValidator.Validate(model).ThenThrow(model);

                var user = Mapper.Map<RegisterModel, User>(model);

                user.PasswordHash = ComputeSha256Hash(model.Password);
                user.JoinDate = DateTime.Today;
                user.UserRoles = new List<UserRole>
                {
                    new UserRole { RoleId = (int)RoleTypes.User }
                };
                user.WantsNotifications = true;
                user.ConfirmedEmail = false;
                uow.Users.Insert(user);
                uow.SaveChanges();

                var token = TokenService.CreateConfirmationEmailToken(uow);
                ImageService.SetStockImage(user, uow);
                MailSenderService.SendConfirmationEmail(user.Id, user.Email, token.Id);
            });
        }

        public void DeleteUnconfirmedUsers()
        {
            ExecuteInTransaction(uow =>
            {
                var pastDateTime = DateTime.Now.AddDays(-3);
                var users = uow.Users.Get()
                    .Where(u => u.ConfirmedEmail == false && u.JoinDate >= pastDateTime);

                foreach (var user in users)
                {
                    uow.Users.Delete(user);
                }
                uow.SaveChanges();
            });
        }

        public List<ListItem<string, Guid>> GetUsers()
        {
            return UnitOfWork.Users.Get()
                .Select(u => new ListItem<string, Guid>
                {
                    Text = $"{u.UserName}",
                    Value = u.Id
                })
                .ToList();
        }
    }
}
