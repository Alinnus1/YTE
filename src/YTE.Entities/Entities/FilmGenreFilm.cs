using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class FilmGenreFilm : IEntity
    {
        public int GenreId { get; set; }
        public Guid FilmId { get; set; }

        public virtual Film Film { get; set; }
        public virtual FilmGenre Genre { get; set; }
    }
}
