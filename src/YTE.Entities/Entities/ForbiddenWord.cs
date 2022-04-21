using YTE.Common;

namespace YTE.Entities
{
    public partial class ForbiddenWord : IEntity
    {
        public int Id { get; set; }
        public string Word { get; set; }
    }
}
