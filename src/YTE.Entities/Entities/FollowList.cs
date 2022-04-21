using System;
using YTE.Common;

namespace YTE.Entities
{
    public partial class FollowList : IEntity
    {
        public Guid FollowerUserId { get; set; }
        public Guid FollowedUserId { get; set; }
        public DateTime Date { get; set; }

        public virtual User FollowedUser { get; set; }
        public virtual User FollowerUser { get; set; }
    }
}
