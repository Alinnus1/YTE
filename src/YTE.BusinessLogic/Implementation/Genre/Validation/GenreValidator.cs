using FluentValidation;
using System.Linq;
using YTE.BusinessLogic.Implementation.Genre.Model;
using YTE.DataAccess;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.Genre.Validation
{
    public class GenreValidator : AbstractValidator<CreateGenreModel>
    {
        private readonly UnitOfWork uow;
        public GenreValidator(UnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Required field!")
                .MinimumLength(3).WithMessage("Genre name must have at least 3 letters!")
                .MaximumLength(50).WithMessage("Genre name can not be longer than 50 letters!")
                .Matches(@"^[a-zA-Z- ]*$").WithMessage("Genre name can have only letters!")
                .Must(IsUnique).WithMessage("Genre already added!");

        }
        private bool IsUnique(CreateGenreModel model, string name)
        {
            var result = false;
            switch (model.GenreType)
            {
                case (int)GenreType.MangaGenre:
                    var mangaGenres = uow.MangaGenres.Get()
                            .Select(mg => mg.Name)
                            .ToList();
                    result = !mangaGenres.Contains(name);
                    break;
                case (int)GenreType.FilmGenre:
                    var filmGenres = uow.FilmGenres.Get()
                            .Select(fg => fg.Name)
                            .ToList();
                    result = !filmGenres.Contains(name);
                    break;
                case (int)GenreType.VideoGameGenre:
                    var videoGameGenres = uow.VideoGameGenres.Get()
                            .Select(vg => vg.Name)
                            .ToList();
                    result = !videoGameGenres.Contains(name);
                    break;
                case (int)GenreType.BookGenre:
                    var bookGenres = uow.BookGenres.Get()
                            .Select(bg => bg.Name)
                            .ToList();
                    result = !bookGenres.Contains(name);
                    break;
                default:

                    break;

            }
            return result;
        }
    }
}
