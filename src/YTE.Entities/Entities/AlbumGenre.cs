using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class AlbumGenre : IEntity
    {
        public AlbumGenre()
        {
            AlbumGenreAlbums = new HashSet<AlbumGenreAlbum>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AlbumGenreAlbum> AlbumGenreAlbums { get; set; }
    }
}
