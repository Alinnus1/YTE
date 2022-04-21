using FluentValidation;
using System;
using System.Linq;
using YTE.BusinessLogic.Implementation.ArtReview.Model;
using YTE.DataAccess;

namespace YTE.BusinessLogic.Implementation.ArtReview.Validation
{
    public class ArtReviewValidator : AbstractValidator<IArtReviewModel>
    {
        private readonly UnitOfWork uow;
        public ArtReviewValidator(UnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            RuleFor(x => x.Score)
                .NotEmpty().WithMessage("Required field!")
                .ScalePrecision(1, 3).WithMessage("Score must be of of format n.d or 10!")
                .GreaterThanOrEqualTo(0).WithMessage("Score can not be lower than 0!")
                .LessThanOrEqualTo(10).WithMessage("Score can not be greater than 10!");

            RuleFor(x => x.ExperiencedAt)
                .NotEmpty().WithMessage("Required field!")
                .Must(BetweenRange).WithMessage("Date of experience must be between the release date and present!");

            RuleFor(x => x.Review)
                .Must(FilterForbiddenWords).WithMessage("You are not allowed to use bad mannered words!");
        }

        private bool BetweenRange(IArtReviewModel model, DateTime date)
        {
            DateTime dateMin = uow.ArtObjects.Get()
                                .Where(a => a.Id == model.ArtObjectId)
                                .Select(a => a.ReleaseDate)
                                .FirstOrDefault();
            DateTime dateMax = DateTime.Now;
            int result1 = DateTime.Compare(dateMin, date);
            int result2 = DateTime.Compare(date, dateMax);

            if (result1 <= 0 && result2 <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool FilterForbiddenWords(IArtReviewModel model, string review)
        {
            var forbiddenWords = uow.ForbiddenWords.Get()
                                    .Select(f => f.Word)
                                    .ToList();
            var hasNy = forbiddenWords.Any(c => review.Contains(c, StringComparison.OrdinalIgnoreCase));

            return !hasNy;
        }
    }
}
