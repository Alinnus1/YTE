using FluentValidation;
using System;
using YTE.BusinessLogic.Implementation.Manga.Model;

namespace YTE.BusinessLogic.Implementation.Manga.Validation
{
    public class MangaValidator : AbstractValidator<IMangaModel>
    {
        public MangaValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Manga name must have at least 3 letters!")
                .MaximumLength(100).WithMessage("Manga name can not be longer than 100 letters!");

            RuleFor(r => r.Author)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Creator name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Creator name can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Creator name can have only letters!");

            RuleFor(r => r.Language)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Language must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Language can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Language can have only letters!"); ;

            RuleFor(r => r.NoVolumes)
                .NotEmpty().WithMessage("Required field!")
                .LessThanOrEqualTo(10000).WithMessage("Value can not be greater than 10000!")
                .GreaterThanOrEqualTo(1).WithMessage("Value can not be less than 1!");

            RuleFor(r => r.NoChapters)
                .NotEmpty().WithMessage("Required field!")
                .LessThanOrEqualTo(10000).WithMessage("Value can not be greater than 10000!")
                .GreaterThanOrEqualTo(1).WithMessage("Value can not be less than 1!");

            RuleFor(r => r.ReleaseDate)
                .NotEmpty().WithMessage("Required field!")
                .Must(BetweenRange).WithMessage("Date must be between 1789 and current year!");

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("Required field!");
        }
        private bool BetweenRange(DateTime date)
        {
            DateTime dateMin = new DateTime(1788, 12, 31, 0, 0, 0);
            DateTime dateMax = DateTime.Now;
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
