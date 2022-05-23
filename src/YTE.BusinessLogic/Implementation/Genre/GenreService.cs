using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.Book.Model;
using YTE.BusinessLogic.Implementation.Film.Model;
using YTE.BusinessLogic.Implementation.Genre.Model;
using YTE.BusinessLogic.Implementation.Genre.Validation;
using YTE.BusinessLogic.Implementation.Manga.Model;
using YTE.BusinessLogic.Implementation.VideoGame.Model;
using YTE.Common.DTOS;
using YTE.Common.Extensions;
using YTE.DataAccess;
using YTE.Entities;
using YTE.Entities.Enums;


namespace YTE.BusinessLogic.Implementation.Genre
{
    public class GenreService : BaseService
    {
        private readonly GenreValidator GenreValidator;
        public GenreService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            this.GenreValidator = new GenreValidator(UnitOfWork);
        }
        public List<ListGenreModel> GetGenres()
        {
            var listGenres1 = UnitOfWork.MangaGenres.Get()
                .Select(a => Mapper.Map<MangaGenre, ListGenreModel>(a))
                .ToList();
            var listGenres2 = UnitOfWork.FilmGenres.Get()
                .Select(a => Mapper.Map<FilmGenre, ListGenreModel>(a))
                .ToList();
            var listGenres3 = UnitOfWork.VideoGameGenres.Get()
                .Select(a => Mapper.Map<VideoGameGenre, ListGenreModel>(a))
                .ToList();
            var listGenres4 = UnitOfWork.BookGenres.Get()
                .Select(a => Mapper.Map<BookGenre, ListGenreModel>(a))
                .ToList();

            listGenres2.ForEach(a => listGenres1.Add(a));
            listGenres3.ForEach(a => listGenres1.Add(a));
            listGenres4.ForEach(a => listGenres1.Add(a));

            return listGenres1;
        }

        public List<ListGenreModel> GetGenresSpecific(int id)
        {
            var genreList = new List<ListGenreModel>();
            switch (id)
            {
                case (int)GenreType.FilmGenre:
                    genreList = UnitOfWork.FilmGenres.Get()
                        .Select(a => Mapper.Map<FilmGenre, ListGenreModel>(a))
                        .ToList();
                    break;
                case (int)GenreType.MangaGenre:
                    genreList = UnitOfWork.MangaGenres.Get()
                        .Select(a => Mapper.Map<MangaGenre, ListGenreModel>(a))
                        .ToList();
                    break;
                case (int)GenreType.VideoGameGenre:
                    genreList = UnitOfWork.VideoGameGenres.Get()
                        .Select(a => Mapper.Map<VideoGameGenre, ListGenreModel>(a))
                        .ToList();
                    break;
                case (int)GenreType.BookGenre:
                    genreList = UnitOfWork.BookGenres.Get()
                        .Select(a => Mapper.Map<BookGenre, ListGenreModel>(a))
                        .ToList();
                    break;
                default:
                    break;
            }
            return genreList;
        }

        public void DeleteGenre(int id, int type)
        {
            ExecuteInTransaction(uow =>
            {
                switch (type)
                {
                    case (int)GenreType.MangaGenre:
                        var mangaGenre = uow.MangaGenres.Get()
                                .FirstOrDefault(a => a.Id == id);
                        uow.MangaGenres.Delete(mangaGenre);
                        uow.SaveChanges();
                        break;
                    case (int)GenreType.FilmGenre:
                        var filmGenre = uow.FilmGenres.Get()
                                .FirstOrDefault(a => a.Id == id);
                        uow.FilmGenres.Delete(filmGenre);
                        uow.SaveChanges();
                        break;
                    case (int)GenreType.VideoGameGenre:
                        var videoGameGenre = uow.VideoGameGenres.Get()
                                .FirstOrDefault(a => a.Id == id);
                        uow.VideoGameGenres.Delete(videoGameGenre);
                        uow.SaveChanges();
                        break;
                    case (int)GenreType.BookGenre:
                        var bookGenre = uow.BookGenres.Get()
                                .FirstOrDefault(a => a.Id == id);
                        uow.BookGenres.Delete(bookGenre);
                        uow.SaveChanges();
                        break;
                    default:
                        uow.SaveChanges();
                        break;
                }
            });
        }

        public void CreateNewGenre(CreateGenreModel model)
        {
            ExecuteInTransaction(uow =>
            {
                GenreValidator.Validate(model).ThenThrow(model);
                switch (model.GenreType)
                {
                    case (int)GenreType.MangaGenre:
                        var mangaGenre = Mapper.Map<CreateGenreModel, MangaGenre>(model);
                        uow.MangaGenres.Insert(mangaGenre);
                        uow.SaveChanges();
                        break;
                    case (int)GenreType.FilmGenre:
                        var filmGenre = Mapper.Map<CreateGenreModel, FilmGenre>(model);
                        uow.FilmGenres.Insert(filmGenre);
                        uow.SaveChanges();
                        break;
                    case (int)GenreType.VideoGameGenre:
                        var videoGameGenre = Mapper.Map<CreateGenreModel, VideoGameGenre>(model);
                        uow.VideoGameGenres.Insert(videoGameGenre);
                        uow.SaveChanges();
                        break;
                    case (int)GenreType.BookGenre:
                        var bookGenre = Mapper.Map<CreateGenreModel, BookGenre>(model);
                        uow.BookGenres.Insert(bookGenre);
                        uow.SaveChanges();
                        break;
                    default:
                        uow.SaveChanges();
                        break;
                }
            });
        }

        public List<ListItem<string, int>> GetGenreTypes()
        {
            var list = new List<ListItem<string, int>>();
            foreach (int i in Enum.GetValues(typeof(GenreType)))
            {
                var name = Enum.GetName(typeof(GenreType), i);
                list.Add(new ListItem<string, int>
                {
                    Text = name,
                    Value = i
                });

            }
            return list;
        }

        #region VideoGame Genres
        public List<ListItem<string, int>> GetVideoGameGenres()
        {
            return UnitOfWork.VideoGameGenres.Get()
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Name}",
                    Value = g.Id
                })
                .ToList();
        }

        public List<string> GetVideoGameGenresOfGame(Guid id)
        {
            return UnitOfWork.VideoGameGenresVideoGame.Get()
                .Where(g => g.VideoGameId == id)
                .Include(g => g.Genre)
                .Select(g => g.Genre.Name)
                .ToList();
        }

        public List<ListItem<string, int>> GetVideoGameGenresOfGameS(Guid id)
        {
            return UnitOfWork.VideoGameGenresVideoGame.Get()
                .Where(g => g.VideoGameId == id)
                .Include(g => g.Genre)
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Genre.Name}",
                    Value = g.Genre.Id
                })
                .ToList();
        }
        public void CreateVideoGameGenreVideoGame(UnitOfWork uow, Entities.VideoGame videogame, int selectedId)
        {
            var relation = new VideoGameGenreVideoGame()
            {
                GenreId = selectedId,
                VideoGameId = videogame.Id
            };

            uow.VideoGameGenresVideoGame.Insert(relation);
        }

        public void SetVideoGameGenres(EditVideoGameModel model, UnitOfWork uow, Entities.VideoGame videogame)
        {
            var currentGenreIds = uow.VideoGameGenresVideoGame.Get()
                 .Where(a => a.VideoGameId == videogame.Id)
                 .Select(a => a.GenreId)
                .ToList();


            if (model.selectedGenres != null)
            {
                var upComingGenreIds = model.selectedGenres;

                var common = currentGenreIds.Intersect(upComingGenreIds).ToList();
                currentGenreIds.RemoveAll(x => common.Contains(x));
                upComingGenreIds.RemoveAll(x => common.Contains(x));

                DeleteVideoGameGenreVideoGameR(uow, videogame, currentGenreIds);

                foreach (var genreId in upComingGenreIds)
                {
                    CreateVideoGameGenreVideoGame(uow, videogame, genreId);
                }
            }
            else
            {
                DeleteVideoGameGenreVideoGameR(uow, videogame, currentGenreIds);
            }
        }

        public void DeleteVideoGameGenreVideoGameR(UnitOfWork uow, Entities.VideoGame videogame, List<int> currentGenreIds)
        {
            foreach (var genreId in currentGenreIds)
            {
                var x = uow.VideoGameGenresVideoGame.Get()
                        .FirstOrDefault(x => x.VideoGameId == videogame.Id && x.GenreId == genreId);
                uow.VideoGameGenresVideoGame.Delete(x);
            }
        }
        #endregion
        #region Film Genres

        public List<ListItem<string, int>> GetFilmGenres()
        {
            return UnitOfWork.FilmGenres.Get()
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Name}",
                    Value = g.Id
                })
                .ToList();
        }

        public List<string> GetFilmGenresOfFilm(Guid id)
        {
            return UnitOfWork.FilmGenresFilm.Get()
                .Where(g => g.FilmId == id)
                .Include(g => g.Genre)
                .Select(g => g.Genre.Name)
                .ToList();
        }

        public List<ListItem<string, int>> GetFilmGenresOfFilmS(Guid id)
        {
            return UnitOfWork.FilmGenresFilm.Get()
                .Where(g => g.FilmId == id)
                .Include(g => g.Genre)
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Genre.Name}",
                    Value = g.Genre.Id
                })
                .ToList();
        }

        public void CreateFilmGenreFilm(UnitOfWork uow, Entities.Film film, int selectedId)
        {
            var relation = new FilmGenreFilm()
            {
                GenreId = selectedId,
                FilmId = film.Id
            };

            uow.FilmGenresFilm.Insert(relation);
        }

        public void SetFilmGenres(EditFilmModel model, UnitOfWork uow, Entities.Film film)
        {

            var currentGenreIds = uow.FilmGenresFilm.Get()
                 .Where(a => a.FilmId == film.Id)
                 .Select(a => a.GenreId)
                .ToList();

            if (model.selectedGenres != null)
            {
                var upComingGenreIds = model.selectedGenres;
                var common = currentGenreIds.Intersect(upComingGenreIds).ToList();
                currentGenreIds.RemoveAll(x => common.Contains(x));
                upComingGenreIds.RemoveAll(x => common.Contains(x));
                DeleteFilmGenreFilmR(uow, film, currentGenreIds);

                foreach (var genreId in upComingGenreIds)
                {
                    CreateFilmGenreFilm(uow, film, genreId);
                }
            }
            else
            {
                DeleteFilmGenreFilmR(uow, film, currentGenreIds);
            }
        }

        public void DeleteFilmGenreFilmR(UnitOfWork uow, Entities.Film film, List<int> currentGenreIds)
        {
            foreach (var genreId in currentGenreIds)
            {
                var x = uow.FilmGenresFilm.Get()
                        .FirstOrDefault(x => x.FilmId == film.Id && x.GenreId == genreId);
                uow.FilmGenresFilm.Delete(x);
            }
        }
        #endregion
        #region Manga Genres

        public List<ListItem<string, int>> GetMangaGenres()
        {
            return UnitOfWork.MangaGenres.Get()
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Name}",
                    Value = g.Id
                })
                .ToList();
        }

        public List<ListItem<string, int>> GetMangaGenresOfMangaS(Guid id)
        {
            return UnitOfWork.MangaGenresManga.Get()
                .Where(g => g.MangaId == id)
                .Include(g => g.Genre)
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Genre.Name}",
                    Value = g.Genre.Id
                })
                .ToList();
        }

        public List<string> GetMangaGenresOfManga(Guid id)
        {
            return UnitOfWork.MangaGenresManga.Get()
                .Where(g => g.MangaId == id)
                .Include(g => g.Genre)
                .Select(g => g.Genre.Name)
                .ToList();
        }

        public void CreateMangaGenreManga(UnitOfWork uow, Entities.Manga manga, int selectedId)
        {
            var relation = new MangaGenreManga()
            {
                GenreId = selectedId,
                MangaId = manga.Id
            };

            uow.MangaGenresManga.Insert(relation);
        }

        public void SetMangaGenres(EditMangaModel model, UnitOfWork uow, Entities.Manga manga)
        {
            var currentGenreIds = uow.MangaGenresManga.Get()
                 .Where(a => a.MangaId == manga.Id)
                 .Select(a => a.GenreId)
                .ToList();

            if (model.selectedGenres != null)
            {
                var upComingGenreIds = model.selectedGenres;
                var common = currentGenreIds.Intersect(upComingGenreIds).ToList();
                currentGenreIds.RemoveAll(x => common.Contains(x));
                upComingGenreIds.RemoveAll(x => common.Contains(x));
                DeleteMangaGenreMangaR(uow, manga, currentGenreIds);

                foreach (var genreId in upComingGenreIds)
                {
                    CreateMangaGenreManga(uow, manga, genreId);
                }
            }
            else
            {
                DeleteMangaGenreMangaR(uow, manga, currentGenreIds);
            }
        }

        private void DeleteMangaGenreMangaR(UnitOfWork uow, Entities.Manga manga, List<int> currentGenreIds)
        {
            foreach (var genreId in currentGenreIds)
            {
                var x = uow.MangaGenresManga.Get()
                        .FirstOrDefault(x => x.MangaId == manga.Id && x.GenreId == genreId);
                uow.MangaGenresManga.Delete(x);
            }
        }
        #endregion
        #region Book Genres
        public List<ListItem<string, int>> GetBookGenres()
        {
            return UnitOfWork.BookGenres.Get()
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Name}",
                    Value = g.Id
                })
                .ToList();
        }

        public List<ListItem<string, int>> GetBookGenresOfBookS(Guid id)
        {
            return UnitOfWork.BookGenresBook.Get()
                .Where(g => g.BookId == id)
                .Include(g => g.Genre)
                .Select(g => new ListItem<string, int>
                {
                    Text = $"{g.Genre.Name}",
                    Value = g.Genre.Id
                })
                .ToList();
        }

        public List<string> GetBookGenresOfBook(Guid id)
        {
            return UnitOfWork.BookGenresBook.Get()
                .Where(g => g.BookId == id)
                .Include(g => g.Genre)
                .Select(g => g.Genre.Name)
                .ToList();
        }

        public void CreateBookGenreBook(UnitOfWork uow, Entities.Book book, int selectedId)
        {
            var relation = new BookGenreBook()
            {
                GenreId = selectedId,
                BookId = book.Id
            };

            uow.BookGenresBook.Insert(relation);
        }

        public void SetBookGenres(EditBookModel model, UnitOfWork uow, Entities.Book book)
        {
            var currentGenreIds = uow.BookGenresBook.Get()
                 .Where(a => a.BookId == book.Id)
                 .Select(a => a.GenreId)
                .ToList();

            if (model.selectedGenres != null)
            {
                var upComingGenreIds = model.selectedGenres;
                var common = currentGenreIds.Intersect(upComingGenreIds).ToList();
                currentGenreIds.RemoveAll(x => common.Contains(x));
                upComingGenreIds.RemoveAll(x => common.Contains(x));
                DeleteBookGenreBookR(uow, book, currentGenreIds);

                foreach (var genreId in upComingGenreIds)
                {
                    CreateBookGenreBook(uow, book, genreId);
                }
            }
            else
            {
                DeleteBookGenreBookR(uow, book, currentGenreIds);
            }
        }

        private void DeleteBookGenreBookR(UnitOfWork uow, Entities.Book book, List<int> currentGenreIds)
        {
            foreach (var genreId in currentGenreIds)
            {
                var x = uow.BookGenresBook.Get()
                        .FirstOrDefault(x => x.BookId == book.Id && x.GenreId == genreId);
                uow.BookGenresBook.Delete(x);
            }
        }

        #endregion
    }
}
