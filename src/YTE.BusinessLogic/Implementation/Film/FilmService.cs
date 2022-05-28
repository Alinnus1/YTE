using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ArtObject;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.FavoriteList;
using YTE.BusinessLogic.Implementation.Film.Model;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.Film.Validation;
using YTE.BusinessLogic.Implementation.WatchList;
using YTE.Common.DTOS;
using YTE.Common.Exceptions;
using YTE.Common.Extensions;
using YTE.DataAccess;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.Film
{
    public class FilmService : BaseService
    {
        private readonly FilmValidator FilmValidator;
        private readonly ImageService ImageService;
        private readonly GenreService GenreService;
        private readonly FavoriteListService FavoriteService;
        private readonly WatchListService WatchListService;
        private readonly ArtReviewService ArtReviewService;
        private readonly ArtObjectService ArtObjectService;

        public FilmService(ServiceDependencies serviceDependencies, ImageService imageService, GenreService genreService, FavoriteListService favoriteListService, WatchListService watchListService, ArtReviewService artReviewService, ArtObjectService artObjectService) : base(serviceDependencies)
        {
            this.FilmValidator = new FilmValidator();
            this.ImageService = imageService;
            this.GenreService = genreService;
            this.FavoriteService = favoriteListService;
            this.WatchListService = watchListService;
            this.ArtReviewService = artReviewService;
            this.ArtObjectService = artObjectService;
        }

        public List<string> GetFilmAttributes()
        {
            return Enum.GetNames(typeof(FilmAttributes))
                .ToList();
        }

        public void CreateNewFilm(CreateFilmModel model)
        {
            ExecuteInTransaction(uow =>
            {
                FilmValidator.Validate(model).ThenThrow(model);

                var artObject = Mapper.Map<CreateFilmModel, Entities.ArtObject>(model);
                ImageService.SetStockPosterBackground(uow, artObject);
                artObject.TypeId = (int)ArtObjectTypes.Film;
                uow.ArtObjects.Insert(artObject);

                var film = Mapper.Map<CreateFilmModel, Entities.Film>(model);
                film.Id = artObject.Id;
                uow.Films.Insert(film);
                if (model.selectedGenres != null)
                {
                    foreach (var selectedId in model.selectedGenres)
                    {
                        GenreService.CreateFilmGenreFilm(uow, film, selectedId);
                    }
                }

                uow.SaveChanges();
            });
        }

        public PaginatedList<ListFilmModel> GetFilmsFilter(string searchString, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "";
            }

            var films = UnitOfWork.Films.Get()
                .Include(a => a.ArtObject)
                .Include(a => a.ArtObject.Poster)
                .Where(a => a.ArtObject.IsDeleted == false && a.ArtObject.Name.Contains(searchString))
                .Select(a => Mapper.Map<Entities.Film, ListFilmModel>(a));
            var paginatedFilms = PaginatedList<ListFilmModel>.Create(films, pageNumber, 10);

            paginatedFilms.ForEach(f =>
            {
                f.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, f.Id);
                f.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, f.Id);
                f.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, f.Id);
                f.Average = ArtReviewService.GetAverageOfArt(f.Id);
                f.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(f.Id);
            });

            return paginatedFilms;
        }

        public DetailsFilmModel DetailsFilm(Guid id)
        {
            var film = UnitOfWork.Films.Get()
                .Include(a => a.ArtObject)
                .Include(a => a.ArtObject.Background)
                .Include(a => a.ArtObject.Poster)
                .FirstOrDefault(a => a.Id == id);

            if (film == null)
            {
                throw new NotFoundErrorException("User Not Found");
            }

            var filmDetails = Mapper.Map<Entities.Film, DetailsFilmModel>(film);
            filmDetails.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, filmDetails.Id);
            filmDetails.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, filmDetails.Id);
            filmDetails.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, filmDetails.Id);
            filmDetails.MostNegativeReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", false);
            filmDetails.MostPositiveReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", true);
            filmDetails.RecentReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Date", true);
            filmDetails.Average = ArtReviewService.GetAverageOfArt(id);
            filmDetails.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(id);

            return filmDetails;
        }

        public string GetFilmName(Guid id)
        {
            return UnitOfWork.Films.Get()
                .Include(f => f.ArtObject)
                .Where(f => f.Id == id)
                .Select(f => f.ArtObject.Name)
                .FirstOrDefault();
        }

        public EditFilmModel EditFilm(Guid id)
        {
            var film = UnitOfWork.Films.Get()
                .Include(a => a.ArtObject)
                .FirstOrDefault(a => a.Id == id);

            return Mapper.Map<Entities.Film, EditFilmModel>(film);
        }

        public void UpdateFilm(EditFilmModel model)
        {
            ExecuteInTransaction(uow =>
            {
                FilmValidator.Validate(model).ThenThrow(model);
                var artObject = ArtObjectService.EditArtObject(model, uow);

                ImageService.SetPoster(model, uow, artObject);
                ImageService.SetBackground(model, uow, artObject);

                var film = Mapper.Map<EditFilmModel, Entities.Film>(model);

                uow.ArtObjects.Update(artObject);
                uow.Films.Update(film);
                GenreService.SetFilmGenres(model, uow, film);
                uow.SaveChanges();
            });
        }

        public void DeleteFilm(Guid id)
        {
            ExecuteInTransaction(uow =>
            {
                var artObject = UnitOfWork.ArtObjects.Get()
                .FirstOrDefault(a => a.Id == id);
                artObject.IsDeleted = true;

                uow.ArtObjects.Update(artObject);
                uow.SaveChanges();
            });
        }
    }
}
