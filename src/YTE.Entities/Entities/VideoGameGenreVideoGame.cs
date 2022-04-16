using System;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class VideoGameGenreVideoGame: IEntity
    {
        public int GenreId { get; set; }
        public Guid VideoGameId { get; set; }

        public virtual VideoGameGenre Genre { get; set; }
        public virtual VideoGame VideoGame { get; set; }
    }
}
