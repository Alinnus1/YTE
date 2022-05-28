using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class FilmGenre : IEntity
    {
        public FilmGenre()
        {
            FilmGenreFilms = new HashSet<FilmGenreFilm>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FilmGenreFilm> FilmGenreFilms { get; set; }
    }
}
