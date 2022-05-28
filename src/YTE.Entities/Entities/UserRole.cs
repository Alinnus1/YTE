using System;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class UserRole : IEntity
    {
        public int RoleId { get; set; }
        public Guid UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
