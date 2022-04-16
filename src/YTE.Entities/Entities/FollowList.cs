using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
