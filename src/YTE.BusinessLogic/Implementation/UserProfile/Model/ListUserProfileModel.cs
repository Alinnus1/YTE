using System;

namespace YTE.BusinessLogic.Implementation.UserProfile.Model
{
    public class ListUserProfileModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public DateTime JoinDate { get; set; }
        public int NoReviews { get; set; }
        public bool Followed { get; set; }
    }
}
