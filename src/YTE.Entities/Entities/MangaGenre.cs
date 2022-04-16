using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class MangaGenre : IEntity
    {
        public MangaGenre()
        {
            MangaGenreMangas = new HashSet<MangaGenreManga>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MangaGenreManga> MangaGenreMangas { get; set; }
    }
}
