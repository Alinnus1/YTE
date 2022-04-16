using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.ForbiddenWord.Model;
using YTE.DataAccess;

namespace YTE.BusinessLogic.Implementation.ForbiddenWord.Validation
{
    public class ForbiddenWordValidator :AbstractValidator<CreateForbiddenWordModel>
    {
        private readonly UnitOfWork uow;
        public ForbiddenWordValidator(UnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            RuleFor(a => a.Word)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(2).WithMessage("Forbidden word must have at least 2 letters!")
                .MaximumLength(100).WithMessage("Forbidden can not be longer than 100 letters!")
                .Must(IsUnique).WithMessage("This forbidden word is already added!");
                
        }
        private bool IsUnique(string word)
        {
            var words = uow.ForbiddenWords.Get()
                    .Select(f => f.Word)
                    .ToList();
            return !words.Contains(word);
        }
    }
}
