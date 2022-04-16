using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class ArtReview : IEntity
    {
        public ArtReview()
        {
            FavoriteLists = new HashSet<FavoriteList>();
        }
        public Guid ArtObjectId { get; set; }
        public Guid UserId { get; set; }
        public decimal Score { get; set; }
        public DateTime ExperiencedAt { get; set; }
        public string Review { get; set; }
        public DateTime Date { get; set; }
        public virtual ArtObject ArtObject { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<FavoriteList> FavoriteLists { get; set; }
    }
}
