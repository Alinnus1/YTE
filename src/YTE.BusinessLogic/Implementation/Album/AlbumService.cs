using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ArtObject;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.FavoriteList;
using YTE.BusinessLogic.Implementation.Album.Model;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.Album.Validation;
using YTE.BusinessLogic.Implementation.WatchList;
using YTE.Common.DTOS;
using YTE.Common.Exceptions;
using YTE.Common.Extensions;
using YTE.DataAccess;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.Album
{
    public class AlbumService : BaseService
    {
        private readonly AlbumValidator AlbumValidator;
        private readonly ImageService ImageService;
        private readonly GenreService GenreService;
        private readonly FavoriteListService FavoriteService;
        private readonly WatchListService WatchListService;
        private readonly ArtReviewService ArtReviewService;
        private readonly ArtObjectService ArtObjectService;

        public AlbumService(ServiceDependencies serviceDependencies, ImageService imageService, GenreService genreService, FavoriteListService favoriteListService, WatchListService watchListService, ArtReviewService artReviewService, ArtObjectService artObjectService) : base(serviceDependencies)
        {
            this.AlbumValidator = new AlbumValidator();
            this.ImageService = imageService;
            this.GenreService = genreService;
            this.FavoriteService = favoriteListService;
            this.WatchListService = watchListService;
            this.ArtReviewService = artReviewService;
            this.ArtObjectService = artObjectService;
        }

        public List<string> GetAlbumAttributes()
        {
            return Enum.GetNames(typeof(AlbumAttributes))
                .ToList();
        }

        public void CreateNewAlbum(CreateAlbumModel model)
        {
            ExecuteInTransaction(uow =>
            {
                AlbumValidator.Validate(model).ThenThrow(model);

                var artObject = Mapper.Map<CreateAlbumModel, Entities.ArtObject>(model);
                ImageService.SetStockPosterBackground(uow, artObject);
                artObject.TypeId = (int)ArtObjectTypes.Album;
                uow.ArtObjects.Insert(artObject);

                var album = Mapper.Map<CreateAlbumModel, Entities.Album>(model);
                album.Id = artObject.Id;
                uow.Albums.Insert(album);
                if (model.selectedGenres != null)
                {
                    foreach (var selectedId in model.selectedGenres)
                    {
                        GenreService.CreateAlbumGenreAlbum(uow, album, selectedId);
                    }
                }

                uow.SaveChanges();
            });
        }

        public PaginatedList<ListAlbumModel> GetAlbumsFilter(string searchString, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "";
            }

            var albums = UnitOfWork.Albums.Get()
                .Include(a => a.ArtObject)
                .Include(a => a.ArtObject.Poster)
                .Where(a => a.ArtObject.IsDeleted == false && a.ArtObject.Name.Contains(searchString))
                .Select(a => Mapper.Map<Entities.Album, ListAlbumModel>(a));
            var paginatedAlbums = PaginatedList<ListAlbumModel>.Create(albums, pageNumber, 10);

            paginatedAlbums.ForEach(f =>
            {
                f.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, f.Id);
                f.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, f.Id);
                f.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, f.Id);
                f.Average = ArtReviewService.GetAverageOfArt(f.Id);
                f.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(f.Id);
            });

            return paginatedAlbums;
        }

        public DetailsAlbumModel DetailsAlbum(Guid id)
        {
            var album = UnitOfWork.Albums.Get()
                .Include(a => a.ArtObject)
                .Include(a => a.ArtObject.Background)
                .Include(a => a.ArtObject.Poster)
                .FirstOrDefault(a => a.Id == id);

            if (album == null)
            {
                throw new NotFoundErrorException("User Not Found");
            }

            var albumDetails = Mapper.Map<Entities.Album, DetailsAlbumModel>(album);
            albumDetails.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, albumDetails.Id);
            albumDetails.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, albumDetails.Id);
            albumDetails.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, albumDetails.Id);
            albumDetails.MostNegativeReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", false);
            albumDetails.MostPositiveReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", true);
            albumDetails.RecentReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Date", true);
            albumDetails.Average = ArtReviewService.GetAverageOfArt(id);
            albumDetails.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(id);

            return albumDetails;
        }

        public string GetAlbumName(Guid id)
        {
            return UnitOfWork.Albums.Get()
                .Include(f => f.ArtObject)
                .Where(f => f.Id == id)
                .Select(f => f.ArtObject.Name)
                .FirstOrDefault();
        }

        public EditAlbumModel EditAlbum(Guid id)
        {
            var album = UnitOfWork.Albums.Get()
                .Include(a => a.ArtObject)
                .FirstOrDefault(a => a.Id == id);

            return Mapper.Map<Entities.Album, EditAlbumModel>(album);
        }

        public void UpdateAlbum(EditAlbumModel model)
        {
            ExecuteInTransaction(uow =>
            {
                AlbumValidator.Validate(model).ThenThrow(model);
                var artObject = ArtObjectService.EditArtObject(model, uow);

                ImageService.SetPoster(model, uow, artObject);
                ImageService.SetBackground(model, uow, artObject);

                var album = Mapper.Map<EditAlbumModel, Entities.Album>(model);

                uow.ArtObjects.Update(artObject);
                uow.Albums.Update(album);
                GenreService.SetAlbumGenres(model, uow, album);
                uow.SaveChanges();
            });
        }

        public void DeleteAlbum(Guid id)
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
