using System;

#nullable disable

namespace YTE.Entities
{
    public partial class BookGenreBook
    {
        public int GenreId { get; set; }
        public Guid BookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual BookGenre Genre { get; set; }
    }
}
