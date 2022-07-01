using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ArtObject;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.FavoriteList;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.Manga.Model;
using YTE.BusinessLogic.Implementation.Manga.Validation;
using YTE.BusinessLogic.Implementation.WatchList;
using YTE.Common.DTOS;
using YTE.Common.Exceptions;
using YTE.Common.Extensions;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.Manga
{
    public class MangaService : BaseService
    {
        private readonly MangaValidator MangaValidator;
        private readonly ImageService ImageService;
        private readonly GenreService GenreService;
        private readonly FavoriteListService FavoriteService;
        private readonly WatchListService WatchListService;
        private readonly ArtReviewService ArtReviewService;
        private readonly ArtObjectService ArtObjectService;

        public MangaService(ServiceDependencies serviceDependencies, ImageService imageService, GenreService genreService, FavoriteListService favoriteListService, WatchListService watchListService, ArtReviewService artReviewService, ArtObjectService artObjectService) : base(serviceDependencies)
        {
            this.MangaValidator = new MangaValidator();
            this.ImageService = imageService;
            this.GenreService = genreService;
            this.FavoriteService = favoriteListService;
            this.WatchListService = watchListService;
            this.ArtReviewService = artReviewService;
            this.ArtObjectService = artObjectService;
        }

        public List<ListMangaModel> GetMangasFilter(string searchString, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "";
            }
            var mangas = UnitOfWork.Mangas.Get()
                   .Include(a => a.ArtObject)
                   .Include(a => a.ArtObject.Poster)
                   .Where(a => a.ArtObject.IsDeleted == false && a.ArtObject.Name.Contains(searchString))
                   .Select(a => Mapper.Map<Entities.Manga, ListMangaModel>(a));

            var paginatedMangas = PaginatedList<ListMangaModel>.Create(mangas, pageNumber, 10);
            paginatedMangas.ForEach(a =>
            {
                a.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, a.Id);
                a.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, a.Id);
                a.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, a.Id);
                a.Average = ArtReviewService.GetAverageOfArt(a.Id);
                a.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(a.Id);
            });

            return paginatedMangas;
        }

        public List<string> GetMangaAttributes()
        {
            return Enum.GetNames(typeof(MangaAttributes))
                .ToList();
        }

        public void CreateNewManga(CreateMangaModel model)
        {
            ExecuteInTransaction(uow =>
            {
                MangaValidator.Validate(model).ThenThrow(model);

                var artObject = Mapper.Map<CreateMangaModel, Entities.ArtObject>(model);
                ImageService.SetStockPosterBackground(uow, artObject);
                artObject.TypeId = (int)ArtObjectTypes.Manga;
                uow.ArtObjects.Insert(artObject);

                var manga = Mapper.Map<CreateMangaModel, Entities.Manga>(model);
                manga.Id = artObject.Id;
                uow.Mangas.Insert(manga);

                if (model.selectedGenres != null)
                {
                    foreach (var selectedId in model.selectedGenres)
                    {

                        GenreService.CreateMangaGenreManga(uow, manga, selectedId);
                    }
                }
                uow.SaveChanges();
            });
        }

        public DetailsMangaModel DetailsManga(Guid id)
        {
            var manga = UnitOfWork.Mangas.Get()
                .Include(a => a.ArtObject)
                .Include(a => a.ArtObject.Background)
                .Include(a => a.ArtObject.Poster)
                .FirstOrDefault(a => a.Id == id);

            if (manga == null)
            {
                throw new NotFoundErrorException("Manga Not Found");
            }

            var detailsManga = Mapper.Map<Entities.Manga, DetailsMangaModel>(manga);
            detailsManga.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, detailsManga.Id);
            detailsManga.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, detailsManga.Id);
            detailsManga.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, detailsManga.Id);
            detailsManga.MostNegativeReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", false);
            detailsManga.MostPositiveReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", true);
            detailsManga.RecentReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Date", true);
            detailsManga.Average = ArtReviewService.GetAverageOfArt(id);
            detailsManga.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(id);
            detailsManga.IsReviewedByCurrentUser = ArtReviewService.IsArtReviewedByUser(id, CurrentUser.UserName);

            return detailsManga;
        }
        public EditMangaModel EditManga(Guid id)
        {
            var manga = UnitOfWork.Mangas.Get()
               .Include(a => a.ArtObject)
               .FirstOrDefault(a => a.Id == id);

            return Mapper.Map<Entities.Manga, EditMangaModel>(manga);
        }

        public void UpdateManga(EditMangaModel model)
        {
            ExecuteInTransaction(uow =>
            {
                MangaValidator.Validate(model).ThenThrow(model);
                var artObject = ArtObjectService.EditArtObject(model, uow);
                var manga = Mapper.Map<EditMangaModel, Entities.Manga>(model);

                ImageService.SetPoster(model, uow, artObject);
                ImageService.SetBackground(model, uow, artObject);

                uow.ArtObjects.Update(artObject);
                uow.Mangas.Update(manga);

                GenreService.SetMangaGenres(model, uow, manga);
                uow.SaveChanges();
            });
        }

        public void DeleteManga(Guid id)
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
