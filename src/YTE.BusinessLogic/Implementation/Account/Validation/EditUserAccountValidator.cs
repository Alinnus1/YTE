using FluentValidation;
using System;
using System.Linq;
using YTE.BusinessLogic.Implementation.Account.Model;
using YTE.DataAccess;

namespace YTE.BusinessLogic.Implementation.Account.Validation
{
    public class EditUserAccountValidator : AbstractValidator<EditUserAccountModel>
    {
        private readonly UnitOfWork uow;
        public EditUserAccountValidator(UnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Required field!")
                .Must(NotAlreadyExist).WithMessage("Email is already being used by somebody else!")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);

            RuleFor(x => x.ConfirmPassword)
               .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("UserName must have at least 3 letters!")
                .MaximumLength(50).WithMessage("UserName can not be longer than 50 letters!");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Name can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Name can have only letters!");

            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Name can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Pronoun can have only letters!");

            RuleFor(r => r.Age)
                .NotEmpty().WithMessage("Required field!")
                .Must(BetweenRange).WithMessage("Date must be between 1900 and 2020!");

            RuleFor(r => r.GenderId)
                .NotEmpty().WithMessage("Required field!");
        }

        private bool NotAlreadyExist(EditUserAccountModel model, string email)
        {
            var emailInitial = uow.Users.Get()
                    .FirstOrDefault(u => u.Id == model.Id).Email;
            var emails = uow.Users.Get()
                        .Select(u => u.Email)
                        .ToList();


            emails.Remove(emailInitial);

            return !emails.Contains(email);
        }

        private bool BetweenRange(DateTime date)
        {
            DateTime dateMin = new DateTime(1900, 1, 1, 0, 0, 0);
            DateTime dateMax = new DateTime(2020, 1, 1, 0, 0, 0);
            int result1 = DateTime.Compare(dateMin, date);
            int result2 = DateTime.Compare(date, dateMax);

            if (result1 < 0 && result2 < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
