using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.ArtReview.Model
{
    public class ListArtReviewModel 
    {

        public Guid ArtObjectId { get; set; }
        public string ArtName { get; set; }
        // puteam sa fac peste acest model inca un model care sa mi mentina proprietatile comune ca sa elimin redundanta
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string Review { get; set; }
        public DateTime ExperiencedAt { get; set; }
        public DateTime Date { get; set; }
        public decimal Score { get; set; }

    }
}