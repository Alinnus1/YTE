using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class Film : IEntity
    {
        public Film()
        {
            FilmGenreFilms = new HashSet<FilmGenreFilm>();
        }

        public Guid Id { get; set; }
        public TimeSpan Length { get; set; }
        public string Studio { get; set; }

        public virtual ArtObject ArtObject { get; set; }
        public virtual ICollection<FilmGenreFilm> FilmGenreFilms { get; set; }
    }
}
