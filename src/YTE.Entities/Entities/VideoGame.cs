using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class VideoGame : IEntity
    {
        public VideoGame()
        {
            VideoGameGenreVideoGames = new HashSet<VideoGameGenreVideoGame>();
        }

        public Guid Id { get; set; }
        public string Esrbrating { get; set; }
        public bool IsMultiplayer { get; set; }

        public virtual ArtObject ArtObject { get; set; }
        public virtual ICollection<VideoGameGenreVideoGame> VideoGameGenreVideoGames { get; set; }
    }
}
