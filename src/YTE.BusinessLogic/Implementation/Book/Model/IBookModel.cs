using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Book.Model
{
    public interface IBookModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public int NoPages { get; set; }
        public int NoChapters { get; set; }
    }
}
