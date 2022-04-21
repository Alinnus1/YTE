using FluentValidation;
using System;
using YTE.BusinessLogic.Implementation.Film.Model;

namespace YTE.BusinessLogic.Implementation.Manga.Validation
{
    public class FilmValidator : AbstractValidator<IFilmModel>
    {
        public FilmValidator()
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
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Language can have only letters!");

            RuleFor(r => r.Studio)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Studio name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Studio name can not be longer than 50 letters!");

            RuleFor(r => r.Length)
                .NotEmpty().WithMessage("Required field!")
                .Must(BetweenRangeSpan).WithMessage("Duration must be between 2 seconds and 23 hours");

            RuleFor(r => r.ReleaseDate)
                .NotEmpty().WithMessage("Required field!")
                .Must(BetweenRange).WithMessage("Release date must be between 1878 and current year!");

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("Required field!");
        }
        private bool BetweenRange(DateTime date)
        {
            DateTime dateMin = new DateTime(1878, 1, 1, 0, 0, 0);
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

        private bool BetweenRangeSpan(TimeSpan timespan)
        {
            TimeSpan timeMin = new TimeSpan(0, 0, 2);
            TimeSpan timeMax = new TimeSpan(23, 0, 0);

            int result1 = TimeSpan.Compare(timeMin, timespan);
            int result2 = TimeSpan.Compare(timespan, timeMax);

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
