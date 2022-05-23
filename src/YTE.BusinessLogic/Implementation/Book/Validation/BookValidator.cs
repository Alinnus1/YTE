using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.Book.Model;

namespace YTE.BusinessLogic.Implementation.Book.Validation
{
    public class BookValidator : AbstractValidator<IBookModel>
    {
        public BookValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Book name must have at least 3 letters!")
                .MaximumLength(100).WithMessage("Book name can not be longer than 100 letters!");

            RuleFor(r => r.Author)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Author name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Author name can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Author name can have only letters!");

            RuleFor(r => r.Language)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Language must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Language can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Language can have only letters!");

            RuleFor(r => r.NoChapters)
                .NotEmpty().WithMessage("Required field!")
                .GreaterThanOrEqualTo(0).WithMessage("Number of chapters can not be less than 0!");

            RuleFor(r => r.NoPages)
                .NotEmpty().WithMessage("Required field!")
                .GreaterThanOrEqualTo(0).WithMessage("Number of pages can not be less than 0!");

            RuleFor(r => r.ReleaseDate)
                .NotEmpty().WithMessage("Required field!");

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("Required field!");
        }
    }
}
