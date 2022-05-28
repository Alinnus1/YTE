using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class Manga : IEntity
    {
        public Manga()
        {
            MangaGenreMangas = new HashSet<MangaGenreManga>();
        }

        public Guid Id { get; set; }
        public int NoVolumes { get; set; }
        public int NoChapters { get; set; }
        public bool IsFinished { get; set; }

        public virtual ArtObject ArtObject { get; set; }
        public virtual ICollection<MangaGenreManga> MangaGenreMangas { get; set; }
    }
}
