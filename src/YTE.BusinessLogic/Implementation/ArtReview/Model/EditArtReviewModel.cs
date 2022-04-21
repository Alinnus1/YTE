using System;

namespace YTE.BusinessLogic.Implementation.ArtReview.Model
{
    public class EditArtReviewModel : IArtReviewModel
    {
        public Guid ArtObjectId { get; set; }
        public string ArtName { get; set; }
        public decimal Score { get; set; }
        public DateTime ExperiencedAt { get; set; }
        public string Review { get; set; }
    }
}
