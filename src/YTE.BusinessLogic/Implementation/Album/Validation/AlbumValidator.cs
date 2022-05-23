using FluentValidation;
using System;
using YTE.BusinessLogic.Implementation.Album.Model;

namespace YTE.BusinessLogic.Implementation.Album.Validation
{
    public class AlbumValidator : AbstractValidator<IAlbumModel>
    {
        public AlbumValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Album name must have at least 3 letters!")
                .MaximumLength(100).WithMessage("Album name can not be longer than 100 letters!");

            RuleFor(r => r.Author)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Artist name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Artist name can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Artist name can have only letters!");

            RuleFor(r => r.Language)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Language must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Language can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Language can have only letters!");

            RuleFor(r => r.NoTracks)
                .NotEmpty().WithMessage("Required field!")
                .GreaterThanOrEqualTo(1).WithMessage("Album must have at least 1 song!");

            RuleFor(r => r.Length)
                .NotEmpty().WithMessage("Required field!");

            RuleFor(r => r.ReleaseDate)
                .NotEmpty().WithMessage("Required field!")
                .Must(BetweenRange).WithMessage("Release date must be between 1889 and current year!");

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("Required field!");
        }
        private bool BetweenRange(DateTime date)
        {
            DateTime dateMin = new DateTime(1889, 1, 1, 0, 0, 0);
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
