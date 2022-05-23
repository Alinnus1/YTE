using System;
using YTE.Common;
#nullable disable

namespace YTE.Entities
{
    public partial class AlbumGenreAlbum : IEntity
    {
        public int GenreId { get; set; }
        public Guid AlbumId { get; set; }

        public virtual AlbumGenre Genre { get; set; }
        public virtual Album Album { get; set; }
    }
}
