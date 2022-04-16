using System;
using YTE.Common;
using YTE.Entities;
using YTE.Entities.Context;


namespace YTE.DataAccess
{
    public class UnitOfWork
    {
        private readonly YTEContext Context;

        public readonly IRepository<User> Users;
        public readonly IRepository<Gender> Genders;
        public readonly IRepository<Film> Films;
        public readonly IRepository<FilmGenre> FilmGenres;
        public readonly IRepository<FilmGenreFilm> FilmGenresFilm;
        public readonly IRepository<ArtObject> ArtObjects;
        public readonly IRepository<Manga> Mangas;
        public readonly IRepository<MangaGenre> MangaGenres;
        public readonly IRepository<MangaGenreManga> MangaGenresManga;
        public readonly IRepository<VideoGame> VideoGames;
        public readonly IRepository<VideoGameGenre> VideoGameGenres;
        public readonly IRepository<VideoGameGenreVideoGame> VideoGameGenresVideoGame;
        public readonly IRepository<Image> Images;
        public readonly IRepository<Role> Roles;
        public readonly IRepository<UserRole> UserRoles;
        public readonly IRepository<ArtReview> ArtReview;
        public readonly IRepository<WatchList> WatchLists;
        public readonly IRepository<FavoriteList> FavoriteLists;
        public readonly IRepository<FollowList> FollowLists;
        public readonly IRepository<Token> Tokens;
        public readonly IRepository<ForbiddenWord> ForbiddenWords;


        public UnitOfWork(YTEContext context)
        {
            this.Context = context;
            this.Users = new BaseRepository<User>(Context);
            this.Genders = new BaseRepository<Gender>(Context);
            this.Films = new BaseRepository<Film>(Context);
            this.FilmGenres = new BaseRepository<FilmGenre>(Context);
            this.FilmGenresFilm = new BaseRepository<FilmGenreFilm>(Context);
            this.ArtObjects = new BaseRepository<ArtObject>(Context);
            this.Mangas = new BaseRepository<Manga>(Context);
            this.MangaGenres = new BaseRepository<MangaGenre>(Context);
            this.MangaGenresManga = new BaseRepository<MangaGenreManga>(Context);
            this.VideoGames = new BaseRepository<VideoGame>(Context);
            this.VideoGameGenres = new BaseRepository<VideoGameGenre>(Context);
            this.VideoGameGenresVideoGame = new BaseRepository<VideoGameGenreVideoGame>(Context);
            this.Images = new BaseRepository<Image>(Context);
            this.Roles = new BaseRepository<Role>(Context);
            this.UserRoles = new BaseRepository<UserRole>(Context);
            this.ArtReview = new BaseRepository<ArtReview>(Context);
            this.WatchLists = new BaseRepository<WatchList>(Context);
            this.FavoriteLists = new BaseRepository<FavoriteList>(Context);
            this.FollowLists = new BaseRepository<FollowList>(Context);
            this.Tokens = new BaseRepository<Token>(Context);
            this.ForbiddenWords = new BaseRepository<ForbiddenWord>(Context);
        }


        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
