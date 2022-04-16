using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class Anime : IEntity
    {
        public Anime()
        {
            AnimeGenreAnimes = new HashSet<AnimeGenreAnime>();
        }

        public Guid Id { get; set; }
        public string Studio { get; set; }
        public int NoSeasons { get; set; }
        public int NoEpisodes { get; set; }
        public TimeSpan AverageEpTime { get; set; }
        public Guid? MangaId { get; set; }
        public bool IsFinished { get; set; }

        public virtual ArtObject ArtObject { get; set; }
        public virtual Manga Manga { get; set; }
        public virtual ICollection<AnimeGenreAnime> AnimeGenreAnimes { get; set; }
    }
}
