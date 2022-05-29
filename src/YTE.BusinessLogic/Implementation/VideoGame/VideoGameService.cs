using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ArtObject;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.FavoriteList;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.VideoGame.Model;
using YTE.BusinessLogic.Implementation.VideoGame.Validation;
using YTE.BusinessLogic.Implementation.WatchList;
using YTE.Common.DTOS;
using YTE.Common.Exceptions;
using YTE.Common.Extensions;
using YTE.DataAccess;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.VideoGame
{
    public class VideoGameService : BaseService
    {
        private readonly VideoGameValidator VideoGameValidator;
        private readonly ImageService ImageService;
        private readonly GenreService GenreService;
        private readonly FavoriteListService FavoriteService;
        private readonly WatchListService WatchListService;
        private readonly ArtReviewService ArtReviewService;
        private readonly ArtObjectService ArtObjectService;

        public VideoGameService(ServiceDependencies serviceDependencies, ImageService imageService, GenreService genreService, FavoriteListService favoriteListService, WatchListService watchListService, ArtReviewService artReviewService, ArtObjectService artObjectService) : base(serviceDependencies)
        {
            this.VideoGameValidator = new VideoGameValidator();
            this.ImageService = imageService;
            this.GenreService = genreService;
            this.FavoriteService = favoriteListService;
            this.WatchListService = watchListService;
            this.ArtReviewService = artReviewService;
            this.ArtObjectService = artObjectService;
        }

        public List<string> GetVideoGameAttributes()
        {
            return Enum.GetNames(typeof(VideoGameAttributes))
                .ToList();
        }

        public void CreateNewVideoGame(CreateVideoGameModel model)
        {
            ExecuteInTransaction(uow =>
            {
                VideoGameValidator.Validate(model).ThenThrow(model);

                var artObject = Mapper.Map<CreateVideoGameModel, Entities.ArtObject>(model);
                ImageService.SetStockPosterBackground(uow, artObject);
                artObject.TypeId = (int)ArtObjectTypes.VideoGame;
                uow.ArtObjects.Insert(artObject);

                var videogame = Mapper.Map<CreateVideoGameModel, Entities.VideoGame>(model);
                videogame.Id = artObject.Id;
                uow.VideoGames.Insert(videogame);

                if (model.selectedGenres != null)
                {
                    foreach (var selectedId in model.selectedGenres)
                    {
                        GenreService.CreateVideoGameGenreVideoGame(uow, videogame, selectedId);
                    }
                }

                uow.SaveChanges();
            });
        }

        public List<ListVideoGameModel> GetVideoGamesFilter(string searchString, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "";
            }
            var videoGames = UnitOfWork.VideoGames.Get()
            .Include(a => a.ArtObject)
            .Include(a => a.ArtObject.Poster)
            .Where(a => a.ArtObject.IsDeleted == false && a.ArtObject.Name.Contains(searchString))
            .Select(a => Mapper.Map<Entities.VideoGame, ListVideoGameModel>(a));
            var paginatedVideogames = PaginatedList<ListVideoGameModel>.Create(videoGames, pageNumber, 10);

            paginatedVideogames.ForEach(a =>
            {
                a.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, a.Id);
                a.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, a.Id);
                a.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, a.Id);
                a.Average = ArtReviewService.GetAverageOfArt(a.Id);
                a.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(a.Id);
            });

            return paginatedVideogames;
        }


        public DetailsVideoGameModel DetailsVideoGame(Guid id)
        {
            var videogame = UnitOfWork.VideoGames.Get()
                .Include(a => a.ArtObject)
                .Include(a => a.ArtObject.Background)
                .Include(a => a.ArtObject.Poster)
                .FirstOrDefault(a => a.Id == id);

            if (videogame == null)
            {
                throw new NotFoundErrorException("VideoGame Not Found");
            }

            var detailsVideoGame = Mapper.Map<Entities.VideoGame, DetailsVideoGameModel>(videogame);

            detailsVideoGame.MostNegativeReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", false);
            detailsVideoGame.MostPositiveReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", true);
            detailsVideoGame.RecentReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Date", true);
            detailsVideoGame.Average = ArtReviewService.GetAverageOfArt(id);
            detailsVideoGame.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(id);
            detailsVideoGame.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, detailsVideoGame.Id);
            detailsVideoGame.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, detailsVideoGame.Id);
            detailsVideoGame.IsReviewedByCurrentUser = ArtReviewService.IsArtReviewedByUser(id, CurrentUser.UserName);

            return detailsVideoGame;
        }
        public EditVideoGameModel EditVideoGame(Guid id)
        {
            var videogame = UnitOfWork.VideoGames.Get()
                .Include(a => a.ArtObject)
                .FirstOrDefault(a => a.Id == id);

            return Mapper.Map<Entities.VideoGame, EditVideoGameModel>(videogame);
        }

        public void UpdateVideoGame(EditVideoGameModel model)
        {
            ExecuteInTransaction(uow =>
            {
                VideoGameValidator.Validate(model).ThenThrow(model);
                var artObject = ArtObjectService.EditArtObject(model, uow);

                ImageService.SetPoster(model, uow, artObject);

                ImageService.SetBackground(model, uow, artObject);

                uow.ArtObjects.Update(artObject);

                var videogame = Mapper.Map<EditVideoGameModel, Entities.VideoGame>(model);

                uow.VideoGames.Update(videogame);
                GenreService.SetVideoGameGenres(model, uow, videogame);
                uow.SaveChanges();

            });
        }


        public void DeleteVideoGame(Guid id)
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
