using System;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class MangaGenreManga : IEntity
    {
        public int GenreId { get; set; }
        public Guid MangaId { get; set; }

        public virtual MangaGenre Genre { get; set; }
        public virtual Manga Manga { get; set; }
    }
}
