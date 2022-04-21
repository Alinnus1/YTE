using System;

namespace YTE.BusinessLogic.Implementation.FollowList.Model
{
    // used to get all the followers of a user
    public class ListFollowersListModel
    {

        public Guid FollowedUserId { get; set; }
        public Guid FollowerUserId { get; set; }
        public string FollowedUserName { get; set; }
        public string FollowerUserName { get; set; }
        public string FollowedImage { get; set; }
        public string FollowerImage { get; set; }

    }
}
