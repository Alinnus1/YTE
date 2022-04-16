using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
