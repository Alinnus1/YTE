using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class Role : IEntity
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
