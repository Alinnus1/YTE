using System.Collections.Generic;

namespace YTE.Entities
{
    public partial class ArtObjectType
    {
        public ArtObjectType()
        {
            ArtObjects = new HashSet<ArtObject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ArtObject> ArtObjects { get; set; }
    }
}
