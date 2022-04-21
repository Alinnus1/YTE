using System;

namespace YTE.BusinessLogic.Implementation.Manga.Model
{
    public class ListMangaModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int NoChapters { get; set; }
        public string Poster { get; set; }
        public decimal Average { get; set; }
        public int NoReviews { get; set; }
        public bool AddedToFavoriteList { get; set; }
        public bool EligibleFavoriteList { get; set; }
        public bool AddedToWatchList { get; set; }

    }
}