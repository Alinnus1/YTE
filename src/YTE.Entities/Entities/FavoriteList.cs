using System;
using YTE.Common;

namespace YTE.Entities
{
    public partial class FavoriteList : IEntity
    {
        public Guid UserId { get; set; }
        public Guid ArtObjectId { get; set; }
        public virtual ArtReview ArtReview { get; set; }
    }
}
