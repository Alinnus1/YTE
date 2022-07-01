using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ArtObject;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.Book.Model;
using YTE.BusinessLogic.Implementation.Book.Validation;
using YTE.BusinessLogic.Implementation.FavoriteList;
using YTE.BusinessLogic.Implementation.Film.Model;
using YTE.BusinessLogic.Implementation.Genre;
using YTE.BusinessLogic.Implementation.Images;
using YTE.BusinessLogic.Implementation.Manga.Validation;
using YTE.BusinessLogic.Implementation.WatchList;
using YTE.Common.DTOS;
using YTE.Common.Exceptions;
using YTE.Common.Extensions;
using YTE.DataAccess;
using YTE.Entities;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.Book
{
    public class BookService : BaseService
    {
        private readonly BookValidator BookValidator;
        private readonly ImageService ImageService;
        private readonly GenreService GenreService;
        private readonly FavoriteListService FavoriteService;
        private readonly WatchListService WatchListService;
        private readonly ArtReviewService ArtReviewService;
        private readonly ArtObjectService ArtObjectService;

        public BookService(ServiceDependencies serviceDependencies, ImageService imageService, GenreService genreService, FavoriteListService favoriteListService, WatchListService watchListService, ArtReviewService artReviewService, ArtObjectService artObjectService) : base(serviceDependencies)
        {
            this.BookValidator = new BookValidator();
            this.ImageService = imageService;
            this.GenreService = genreService;
            this.FavoriteService = favoriteListService;
            this.WatchListService = watchListService;
            this.ArtReviewService = artReviewService;
            this.ArtObjectService = artObjectService;
        }

        public List<string> GetBookAttributes()
        {
            return Enum.GetNames(typeof(BookAttributes)).ToList();
        }

        public void CreateNewBook(CreateBookModel model)
        {
            ExecuteInTransaction(uow =>
            {
                BookValidator.Validate(model).ThenThrow(model);

                var artObject = Mapper.Map<CreateBookModel, Entities.ArtObject>(model);
                ImageService.SetStockPosterBackground(uow, artObject);
                artObject.TypeId = (int)ArtObjectTypes.Book;
                uow.ArtObjects.Insert(artObject);

                var book = Mapper.Map<CreateBookModel, Entities.Book>(model);
                book.Id = artObject.Id;
                uow.Books.Insert(book);

                if (model.selectedGenres != null)
                {
                    foreach (var selectedId in model.selectedGenres)
                    {
                        GenreService.CreateBookGenreBook(uow, book, selectedId);
                    }
                }

                uow.SaveChanges();
            });
        }

        public PaginatedList<ListBookModel> GetBooksFilter(string searchString, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "";
            }

            var books = UnitOfWork.Books.Get()
                    .Include(a => a.ArtObject)
                    .Include(a => a.ArtObject.Poster)
                    .Where(a => a.ArtObject.IsDeleted == false && a.ArtObject.Name.Contains(searchString))
                    .Select(a => Mapper.Map<Entities.Book, ListBookModel>(a));
            var paginatedBooks = PaginatedList<ListBookModel>.Create(books, pageNumber, 10);

            paginatedBooks.ForEach(f =>
            {
                f.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, f.Id);
                f.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, f.Id);
                f.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, f.Id);
                f.Average = ArtReviewService.GetAverageOfArt(f.Id);
                f.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(f.Id);
            });

            return paginatedBooks;
        }

        public DetailsBookModel DetailsBook(Guid id)
        {
            var book = UnitOfWork.Books.Get()
                .Include(a => a.ArtObject)
                .Include(a => a.ArtObject.Background)
                .Include(a => a.ArtObject.Poster)
                .FirstOrDefault(a => a.Id == id);

            if (book == null)
            {
                throw new NotFoundErrorException("User Not Found");
            }

            var bookDetails = Mapper.Map<Entities.Book, DetailsBookModel>(book);
            bookDetails.AddedToFavoriteList = FavoriteService.CheckAdded(CurrentUser.Id, bookDetails.Id);
            bookDetails.EligibleFavoriteList = FavoriteService.CheckExperienced(CurrentUser.Id, bookDetails.Id);
            bookDetails.AddedToWatchList = WatchListService.CheckAdded(CurrentUser.UserName, bookDetails.Id);
            bookDetails.MostNegativeReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", false);
            bookDetails.MostPositiveReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Score", true);
            bookDetails.RecentReviews = ArtReviewService.GetReviewsOfArtForDetails(id, "Date", true);
            bookDetails.Average = ArtReviewService.GetAverageOfArt(id);
            bookDetails.NoReviews = ArtReviewService.GetNumberOfReviewsOfArt(id);
            bookDetails.IsReviewedByCurrentUser = ArtReviewService.IsArtReviewedByUser(id, CurrentUser.UserName);

            return bookDetails;
        }

        public string GetBookName(Guid id)
        {
            return UnitOfWork.Books.Get()
                .Include(f => f.ArtObject)
                .Where(f => f.Id == id)
                .Select(f => f.ArtObject.Name)
                .FirstOrDefault();
        }

        public EditBookModel EditBook(Guid id)
        {
            var book = UnitOfWork.Books.Get()
                .Include(a => a.ArtObject)
                .FirstOrDefault(a => a.Id == id);

            return Mapper.Map<Entities.Book, EditBookModel>(book);
        }

        public void UpdateBook(EditBookModel model)
        {
            ExecuteInTransaction(uow =>
            {
                BookValidator.Validate(model).ThenThrow(model);
                var artObject = ArtObjectService.EditArtObject(model, uow);

                ImageService.SetPoster(model, uow, artObject);
                ImageService.SetBackground(model, uow, artObject);

                var book = Mapper.Map<EditBookModel, Entities.Book>(model);

                uow.ArtObjects.Update(artObject);
                uow.Books.Update(book);
                GenreService.SetBookGenres(model, uow, book);
                uow.SaveChanges();
            });
        }

        public void DeleteBook(Guid id)
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
