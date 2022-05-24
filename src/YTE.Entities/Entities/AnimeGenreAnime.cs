using System;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class AnimeGenreAnime : IEntity
    {
        public int GenreId { get; set; }
        public Guid AnimeId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual AnimeGenre Genre { get; set; }
    }
}
