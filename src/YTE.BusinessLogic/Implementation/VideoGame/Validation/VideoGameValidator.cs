using FluentValidation;
using System;
using YTE.BusinessLogic.Implementation.VideoGame.Model;

namespace YTE.BusinessLogic.Implementation.VideoGame.Validation
{
    public class VideoGameValidator : AbstractValidator<IVideoGameModel>
    {
        public VideoGameValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Manga name must have at least 3 letters!")
                .MaximumLength(100).WithMessage("Manga name can not be longer than 100 letters!");

            RuleFor(r => r.Author)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Developer name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Developer name can not be longer than 50 letters!");

            RuleFor(r => r.Language)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Language must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Language can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Language can have only letters!"); ;
            ;
            RuleFor(r => r.ReleaseDate)
                .NotEmpty().WithMessage("Required field!")
                .Must(BetweenRange).WithMessage("Release date must be between 1958 and current year!");

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("Required field!");

            RuleFor(r => r.Esrbrating)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(2).WithMessage("ESRB Rating must have at least 2 letters!")
                .MaximumLength(20).WithMessage("ESRB Rating can not be longer than 20 letters!");
        }

        private bool BetweenRange(DateTime date)
        {
            DateTime dateMin = new DateTime(1958, 1, 1, 0, 0, 0);
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
