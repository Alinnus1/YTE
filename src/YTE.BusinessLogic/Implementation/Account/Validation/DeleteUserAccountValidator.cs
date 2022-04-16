using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.Account.Model;

namespace YTE.BusinessLogic.Implementation.Account.Validation
{
    public class DeleteUserAccountValidator : AbstractValidator<DeleteUserAccountModel>
    {
        public DeleteUserAccountValidator()
        {
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}
