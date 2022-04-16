using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class Album : IEntity
    {
        public Album()
        {
            AlbumGenreAlbums = new HashSet<AlbumGenreAlbum>();
        }

        public Guid Id { get; set; }
        public TimeSpan Length { get; set; }
        public int NoTracks { get; set; }

        public virtual ArtObject ArtObject { get; set; }
        public virtual ICollection<AlbumGenreAlbum> AlbumGenreAlbums { get; set; }
    }
}
