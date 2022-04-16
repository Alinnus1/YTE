using System;
using System.Collections.Generic;

#nullable disable

namespace YTE.Entities
{
    public partial class AlbumGenreAlbum
    {
        public int GenreId { get; set; }
        public Guid MusicArtId { get; set; }

        public virtual AlbumGenre Genre { get; set; }
        public virtual Album MusicArt { get; set; }
    }
}
