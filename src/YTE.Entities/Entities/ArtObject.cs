using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class ArtObject : IEntity
    {
        public ArtObject()
        {
            ArtReviews = new HashSet<ArtReview>();
            WatchLists = new HashSet<WatchList>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public Guid? BackgroundId { get; set; }
        public Guid? PosterId { get; set; }
        public int? TypeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Image Background { get; set; }
        public virtual Image Poster { get; set; }
        public virtual ArtObjectType Type { get; set; }
        public virtual Album Album { get; set; }
        public virtual Anime Anime { get; set; }
        public virtual Book Book { get; set; }
        public virtual Film Film { get; set; }
        public virtual Manga Manga { get; set; }
        public virtual Tvseries Tvseries { get; set; }
        public virtual VideoGame VideoGame { get; set; }
        public virtual ICollection<ArtReview> ArtReviews { get; set; }
        public virtual ICollection<WatchList> WatchLists { get; set; }

    }
}
