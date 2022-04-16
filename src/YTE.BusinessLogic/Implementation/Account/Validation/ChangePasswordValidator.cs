using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.Account.Model;

namespace YTE.BusinessLogic.Implementation.Account.Validation
{
    public class ChangePasswordValidator : AbstractValidator<IChangePassword>
    {
        public ChangePasswordValidator()
        {

            RuleFor(r => r.NewPassword)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters")
                .MaximumLength(25).WithMessage("Password can not be longer than 25 characters")
                .Matches("[A-Z]").WithMessage("Password must have at least one uppercase letter!")
                .Matches("[a-z]").WithMessage("Password must have at least one lowercase letter!")
                .Matches("[0-9]").WithMessage("Password must have at least a digit!")
                .Matches("[@#$%^&+=!*]").WithMessage("Password must have at least one special character!")
                ;

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
        }
    }
}
