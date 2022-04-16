using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.FavoriteList.Model
{
    public class ListFavoriteListModel
    {
        public Guid UserId { get; set; }
        public Guid ArtObjectId { get; set; }
        public string ArtName { get; set; }
        public string Poster { get; set; }
        public string Review { get; set; }
        public decimal Score { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExperiencedDate { get; set; }
        public string Route { get; set; }
    }
}
