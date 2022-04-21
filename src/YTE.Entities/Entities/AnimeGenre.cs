using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class AnimeGenre : IEntity
    {
        public AnimeGenre()
        {
            AnimeGenreAnimes = new HashSet<AnimeGenreAnime>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AnimeGenreAnime> AnimeGenreAnimes { get; set; }
    }
}
