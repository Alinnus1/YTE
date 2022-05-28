using System;
using System.Collections.Generic;
using YTE.BusinessLogic.Implementation.ArtReview.Model;

namespace YTE.BusinessLogic.Implementation.UserProfile.Model
{
    public class DetailsUserProfileModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public DateTime JoinDate { get; set; }
        public string Image { get; set; }
        public int NoReviews { get; set; }
        public bool Followed { get; set; }
        public int NoFollowers { get; set; }

        public List<ListUserProfileArtReviewModel> RecentReviews { get; set; }
        public List<ListUserProfileArtReviewModel> MostPositiveReviews { get; set; }
        public List<ListUserProfileArtReviewModel> MostNegativeReviews { get; set; }
    }
}
