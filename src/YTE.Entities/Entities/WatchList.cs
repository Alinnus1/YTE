using System;
using YTE.Common;

namespace YTE.Entities
{
    public partial class WatchList : IEntity
    {
        public string UserName { get; set; }
        public Guid ArtObjectId { get; set; }
        public virtual User User { get; set; }
        public virtual ArtObject ArtObject { get; set; }
    }
}
