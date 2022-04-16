using System;
using System.Collections.Generic;
using YTE.Common;
using YTE.Entities;

#nullable disable

namespace YTE.Entities
{
    public partial class VideoGameGenre : IEntity
    {
        public VideoGameGenre()
        {
            VideoGameGenreVideoGames = new HashSet<VideoGameGenreVideoGame>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<VideoGameGenreVideoGame> VideoGameGenreVideoGames { get; set; }
    }
}
