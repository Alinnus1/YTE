using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.Admin.Model;
using YTE.DataAccess;

namespace YTE.BusinessLogic.Implementation.Admin.Validation
{
    public class EditUserValidator : AbstractValidator<EditUserModel>
    {
        private readonly UnitOfWork uow;

        public EditUserValidator(UnitOfWork unitOfWork)
        {
            uow = unitOfWork;

            RuleFor(r => r.Email)
                 .NotEmpty().WithMessage("Required field!")
                 .Must(NotAlreadyExistMail).WithMessage("Email is already being used by somebody else!")
                 .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);

            RuleFor(r => r.UserName)
                .NotEmpty().WithMessage("Required field!")
                .Must(NotAlreadyExistUserName).WithMessage("UserName is already being used by somebody else!");

            RuleFor(r => r.Name)
                .MaximumLength(50).WithMessage("Name can not be longer than 50 letters!");
            RuleFor(r => r.Pronoun)
                .MaximumLength(50).WithMessage("Pronoun can not be longer than 50 letters!");
        }

        private bool NotAlreadyExistMail(EditUserModel model, string email)
        {
            var emails = uow.Users.Get()
                        .Select(u => u.Email)
                        .ToList();
            var currentEmail = uow.Users.Get()
                                .Where(u => u.Id == model.Id)
                                .Select(u => u.Email)
                                .FirstOrDefault();
                    
            emails.Remove(currentEmail);
            return !emails.Contains(email);
        }
        private bool NotAlreadyExistUserName(EditUserModel model, string username)
        {
            var usernames = uow.Users.Get()
                         .Select(u => u.UserName)
                         .ToList();
            var currentUserName = uow.Users.Get()
                                .Where(u => u.Id == model.Id)
                                .Select(u => u.UserName)
                                .FirstOrDefault();
            usernames.Remove(currentUserName);
            return !usernames.Contains(username);
        }

    }
}
