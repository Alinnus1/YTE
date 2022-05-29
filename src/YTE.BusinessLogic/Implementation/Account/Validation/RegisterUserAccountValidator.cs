using FluentValidation;
using System;
using System.Linq;
using YTE.BusinessLogic.Implementation.Account.Model;
using YTE.DataAccess;

namespace YTE.BusinessLogic.Implementation.Account.Validation
{
    public class RegisterUserAccountValidator : AbstractValidator<RegisterModel>
    {
        private readonly UnitOfWork uow;

        public RegisterUserAccountValidator(UnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Required field!")
                .Must(NotAlreadyExist).WithMessage("Email is already being used by somebody else!")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters")
                .MaximumLength(25).WithMessage("Password can not be longer than 25 characters")
                .Matches("[A-Z]").WithMessage("Password must have at least one uppercase letter!")
                .Matches("[a-z]").WithMessage("Password must have at least one lowercase letter!")
                .Matches("[0-9]").WithMessage("Password must have at least a digit!")
                .Matches("[@#$%^&+=!*]").WithMessage("Password must have at least one special character!")
                ;

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Required field!")
                .Must(FilterForbiddenWords).WithMessage("You are not allowed to use bad mannered words!")
                .MinimumLength(3).WithMessage("UserName must have at least 3 letters!")
                .MaximumLength(50).WithMessage("UserName can not be longer than 50 letters!");

            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Name can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Name can have only letters!");

            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Pronoun must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Pronoun can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Pronoun can have only letters!");

            RuleFor(r => r.Age)
                .NotEmpty().WithMessage("Required field!")
                .Must(BetweenRange).WithMessage("Date must be between 1900 and 2020!");

            RuleFor(r => r.GenderId)
                .NotEmpty().WithMessage("Required field!");

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

        private bool NotAlreadyExist(string email)
        {
            var emails = uow.Users.Get()
                        .Select(u => u.Email)
                        .ToList();

            return !emails.Contains(email);
        }
        private bool FilterForbiddenWords(string userName)
        {
            var forbiddenWords = uow.ForbiddenWords.Get()
                                    .Select(f => f.Word)
                                    .ToList();
            var hasNy = forbiddenWords.Any(c => userName.Contains(c, StringComparison.OrdinalIgnoreCase));

            return !hasNy;
        }
    }
}

