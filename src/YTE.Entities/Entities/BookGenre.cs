using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class BookGenre : IEntity
    {
        public BookGenre()
        {
            BookGenreBooks = new HashSet<BookGenreBook>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookGenreBook> BookGenreBooks { get; set; }
    }
}
