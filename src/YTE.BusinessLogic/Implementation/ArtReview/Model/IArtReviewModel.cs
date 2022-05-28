using System;

namespace YTE.BusinessLogic.Implementation.ArtReview.Model
{
    public interface IArtReviewModel
    {
        public Guid ArtObjectId { get; set; }
        public decimal Score { get; set; }
        public DateTime ExperiencedAt { get; set; }
        public string Review { get; set; }
    }
}
