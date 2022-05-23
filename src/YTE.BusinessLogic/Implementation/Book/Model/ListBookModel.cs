using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Book.Model
{
    public class ListBookModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int NoPages { get; set; }
        public int NoChapters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public decimal Average { get; set; }
        public int NoReviews { get; set; }
        public bool AddedToFavoriteList { get; set; }
        public bool EligibleFavoriteList { get; set; }
        public bool AddedToWatchList { get; set; }
    }
}
