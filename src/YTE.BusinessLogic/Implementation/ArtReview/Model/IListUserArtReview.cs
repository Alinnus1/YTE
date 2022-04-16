using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.ArtReview.Model
{
    public interface IListUserArtReview
    {
        public Guid ArtObjectId { get; set; }
        public string ArtName { get; set; }
        public string Review { get; set; }
        public DateTime ExperiencedAt { get; set; }
        public DateTime Date { get; set; }
        public decimal Score { get; set; }
        public string UserName { get; set; }
        public string Route { get; set; }
    }
}
