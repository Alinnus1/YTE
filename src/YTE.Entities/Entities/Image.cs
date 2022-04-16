using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class Image : IEntity
    {
        public Image()
        {
            ArtObjectBackgrounds = new HashSet<ArtObject>();
            ArtObjectPosters = new HashSet<ArtObject>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string ImageName { get; set; }
        public byte[] Content { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ArtObject> ArtObjectBackgrounds { get; set; }
        public virtual ICollection<ArtObject> ArtObjectPosters { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
