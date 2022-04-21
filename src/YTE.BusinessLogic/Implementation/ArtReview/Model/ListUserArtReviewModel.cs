using System;

namespace YTE.BusinessLogic.Implementation.ArtReview.Model
{
    public class ListUserArtReviewModel : IListUserArtReview
    {
        public Guid ArtObjectId { get; set; }
        public string Review { get; set; }
        public DateTime ExperiencedAt { get; set; }
        public DateTime Date { get; set; }

        public decimal Score { get; set; }
        public string ArtName { get; set; }
        public string UserName { get; set; }
        public string Route { get; set; }
    }
}
