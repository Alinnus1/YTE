using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ArtReview.Model;
using YTE.BusinessLogic.Implementation.ArtReview.Validation;
using YTE.BusinessLogic.Implementation.FavoriteList;
using YTE.BusinessLogic.Implementation.WatchList;
using YTE.Common.DTOS;
using YTE.Common.Extensions;

namespace YTE.BusinessLogic.Implementation.ArtReview
{
    public class ArtReviewService : BaseService
    {
        private readonly ArtReviewValidator ArtReviewValidator;
        private readonly FavoriteListService FavoriteService;
        private readonly WatchListService WatchListService;
        public ArtReviewService(ServiceDependencies serviceDependencies, FavoriteListService favoriteListService, WatchListService watchListService) : base(serviceDependencies)
        {
            this.ArtReviewValidator = new ArtReviewValidator(UnitOfWork);
            this.FavoriteService = favoriteListService;
            this.WatchListService = watchListService;
        }

        public void CreateArtReview(CreateArtReviewModel model)
        {
            if (model.Review == null)
            {
                model.Review = "";
            }
            ExecuteInTransaction(uow =>
            {
                ArtReviewValidator.Validate(model).ThenThrow(model);
                var review = Mapper.Map<CreateArtReviewModel, Entities.ArtReview>(model);
                review.UserId = CurrentUser.Id;
                review.Date = DateTime.Now;
                uow.ArtReview.Insert(review);

                WatchListService.Remove(uow, CurrentUser.UserName, review.ArtObjectId);

                uow.SaveChanges();
            });
        }

        public List<ListArtReviewModel> GetReviewsOfArt(Guid id, string type, bool desc, int pageNumber)
        {
            var reviews = UnitOfWork.ArtReview.Get()
                    .Where(a => a.ArtObjectId == id)
                    .Include(a => a.ArtObject)
                    .Where(a => a.ArtObject.IsDeleted == false)
                    .Include(a => a.User)
                        .ThenInclude(u => u.Image)
                    .OrderBy(type, desc)
                    .Select(a => Mapper.Map<Entities.ArtReview, ListArtReviewModel>(a));
            var paginatedReviews = PaginatedList<ListArtReviewModel>.Create(reviews, pageNumber, 10);

            return paginatedReviews;
        }

        public List<ListArtReviewModel> GetReviewsOfArtForDetails(Guid id, string type, bool desc)
        {
            return UnitOfWork.ArtReview.Get()
                    .Where(a => a.ArtObjectId == id)
                    .Include(a => a.ArtObject)
                    .Where(a => a.ArtObject.IsDeleted == false)
                    .Include(a => a.User)
                        .ThenInclude(u => u.Image)
                    .OrderBy(type, desc)
                    .Select(a => Mapper.Map<Entities.ArtReview, ListArtReviewModel>(a))
                    .Take(3)
                    .ToList();
        }

        public List<ListUserProfileArtReviewModel> GetSpecificUserReviewsOfArtForProfile(string username, string column, bool desc)
        {
            return UnitOfWork.ArtReview.Get()
                     .Include(a => a.User)
                     .Include(a => a.ArtObject)
                     .Where(a => a.ArtObject.IsDeleted == false && a.User.UserName == username)
                     .OrderBy(column, desc)
                     .Select(a => Mapper.Map<Entities.ArtReview, ListUserProfileArtReviewModel>(a))
                     .Take(3)
                     .ToList();
        }

        public bool DeleteArtReviewOfCurrent(Guid id, string username)
        {
            if (id == null || username == null)
            {
                return false;
            }
            return ExecuteInTransaction(uow =>
            {
                var review = uow.ArtReview.Get()
                    .Include(a => a.User)
                    .Where(a => a.ArtObjectId == id && a.User.UserName == username)
                    .FirstOrDefault();
                if (CurrentUser.Roles.Contains("Admin") || CurrentUser.UserName == username)
                {
                    uow.ArtReview.Delete(review);
                    uow.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public int GetNumberOfReviewsOfArt(Guid id)
        {
            return UnitOfWork.ArtReview.Get()
                .Where(ar => ar.ArtObjectId == id)
                .Count();
        }

        public EditArtReviewModel EditArtReview(Guid id, string username)
        {
            return UnitOfWork.ArtReview.Get()
                .Include(u => u.User)
                .Include(a => a.ArtObject)
                .Where(a => a.ArtObjectId == id && a.User.UserName == username && a.ArtObject.IsDeleted == false)
                .Select(a => Mapper.Map<Entities.ArtReview, EditArtReviewModel>(a))
                .FirstOrDefault();
        }

        public bool UpdateArtReviewOfCurrent(EditArtReviewModel model, Guid id, string username)
        {
            if (id == null || username == null)
            {
                return false;
            }
            if (model.Review == null)
            {
                model.Review = "";
            }
            return ExecuteInTransaction(uow =>
            {
                ArtReviewValidator.Validate(model).ThenThrow(model);
                var review = uow.ArtReview.Get()
                    .Include(a => a.User)
                    .Where(a => a.ArtObjectId == id && a.User.UserName == username)
                    .FirstOrDefault();

                if (CurrentUser.Roles.Contains("Admin") || CurrentUser.UserName == username)
                {
                    review.Score = model.Score;
                    review.ExperiencedAt = model.ExperiencedAt;
                    review.Review = model.Review;

                    uow.ArtReview.Update(review);
                    uow.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public bool Check(Guid id, string username)
        {
            return UnitOfWork.ArtReview.Get()
                .Include(a => a.User)
                .Any(a => a.ArtObjectId == id && a.User.UserName == username);
        }

        public PaginatedList<ListUserArtReviewModel> GetSpecificUserReviewsOfGames(string username, string type, bool desc, int pageNumber)
        {
            var reviews = UnitOfWork.ArtReview.Get()
                     .Include(a => a.User)
                     .Where(a => a.User.UserName == username)
                     .Include(a => a.ArtObject)
                         .ThenInclude(a => a.VideoGame)
                     .Where(a => a.ArtObject.VideoGame.Id == a.ArtObject.Id && a.ArtObject.IsDeleted == false)
                     .OrderBy(type, desc)
                     .Select(a => Mapper.Map<Entities.ArtReview, ListUserArtReviewModel>(a));

            var paginatedReviews = PaginatedList<ListUserArtReviewModel>.Create(reviews, pageNumber, 10);

            return paginatedReviews;
        }

        public PaginatedList<ListUserArtReviewModel> GetSpecificUserReviewsOfMangas(string username, string type, bool desc, int pageNumber)
        {
            var reviews = UnitOfWork.ArtReview.Get()
                     .Include(a => a.User)
                     .Where(a => a.User.UserName == username)
                     .Include(a => a.ArtObject)
                         .ThenInclude(a => a.Manga)
                     .Where(a => a.ArtObject.Manga.Id == a.ArtObject.Id && a.ArtObject.IsDeleted == false)
                     .OrderBy(type, desc)
                     .Select(a => Mapper.Map<Entities.ArtReview, ListUserArtReviewModel>(a));
            var paginatedReviews = PaginatedList<ListUserArtReviewModel>.Create(reviews, pageNumber, 10);

            return paginatedReviews;
        }

        public PaginatedList<ListUserArtReviewModel> GetSpecificUserReviewsOfFilms(string username, string type, bool desc, int pageNumber)
        {
            var reviews = UnitOfWork.ArtReview.Get()
                    .Include(a => a.User)
                    .Where(a => a.User.UserName == username)
                    .Include(a => a.ArtObject)
                        .ThenInclude(a => a.Film)
                    .Where(a => a.ArtObject.Film.Id == a.ArtObject.Id && a.ArtObject.IsDeleted == false)
                    .OrderBy(type, desc)
                    .Select(a => Mapper.Map<Entities.ArtReview, ListUserArtReviewModel>(a));
            var pagintaedReviews = PaginatedList<ListUserArtReviewModel>.Create(reviews, pageNumber, 10);

            return pagintaedReviews;
        }

        public PaginatedList<ListUserArtReviewModel> GetSpecificUserReviewsOfBooks(string username, string type, bool desc, int pageNumber)
        {
            var reviews = UnitOfWork.ArtReview.Get()
                    .Include(a => a.User)
                    .Where(a => a.User.UserName == username)
                    .Include(a => a.ArtObject)
                        .ThenInclude(a => a.Book)
                    .Where(a => a.ArtObject.Book.Id == a.ArtObject.Id && a.ArtObject.IsDeleted == false)
                    .OrderBy(type, desc)
                    .Select(a => Mapper.Map<Entities.ArtReview, ListUserArtReviewModel>(a));
            var pagintaedReviews = PaginatedList<ListUserArtReviewModel>.Create(reviews, pageNumber, 10);

            return pagintaedReviews;
        }

        public PaginatedList<ListUserArtReviewModel> GetSpecificUserReviewsOfAlbums(string username, string type, bool desc, int pageNumber)
        {
            var reviews = UnitOfWork.ArtReview.Get()
                    .Include(a => a.User)
                    .Where(a => a.User.UserName == username)
                    .Include(a => a.ArtObject)
                        .ThenInclude(a => a.Album)
                    .Where(a => a.ArtObject.Album.Id == a.ArtObject.Id && a.ArtObject.IsDeleted == false)
                    .OrderBy(type, desc)
                    .Select(a => Mapper.Map<Entities.ArtReview, ListUserArtReviewModel>(a));
            var pagintaedReviews = PaginatedList<ListUserArtReviewModel>.Create(reviews, pageNumber, 10);

            return pagintaedReviews;
        }

        public PaginatedList<ListUserArtReviewModel> GetSpecificUserReviewsOfArt(string username, string type, bool desc, int pageNumber)
        {
            var reviews = UnitOfWork.ArtReview.Get()
                    .Include(a => a.User)
                    .Where(a => a.User.UserName == username)
                    .Include(a => a.ArtObject)
                    .Where(a => a.ArtObject.IsDeleted == false)
                    .OrderBy(type, desc)
                    .Select(a => Mapper.Map<Entities.ArtReview, ListUserArtReviewModel>(a));
            var paginatedReviews = PaginatedList<ListUserArtReviewModel>.Create(reviews, pageNumber, 10);

            return paginatedReviews;
        }

        public decimal GetAverageOfArt(Guid id)
        {
            var listScores = UnitOfWork.ArtReview.Get()
                .Include(a => a.ArtObject)
                .Where(a => a.ArtObjectId == id)
                .Select(a => a.Score);

            if (listScores.Count() == 0)
            {
                return 0;
            }
            else
            {
                return Math.Round(listScores.Average(), 1);
            }
        }
    }
}
