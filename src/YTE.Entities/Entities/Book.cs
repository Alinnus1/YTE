using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class Book : IEntity
    {
        public Book()
        {
            BookGenreBooks = new HashSet<BookGenreBook>();
        }

        public Guid Id { get; set; }
        public int NoPages { get; set; }
        public int NoChapters { get; set; }

        public virtual ArtObject ArtObject { get; set; }
        public virtual ICollection<BookGenreBook> BookGenreBooks { get; set; }
    }
}
