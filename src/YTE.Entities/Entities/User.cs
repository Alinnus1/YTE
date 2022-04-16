using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class User : IEntity
    {
        public User()
        {
            ArtReviews = new HashSet<ArtReview>();
            FollowListFollowedUsers = new HashSet<FollowList>();
            FollowListFollowerUsers = new HashSet<FollowList>();
            UserRoles = new HashSet<UserRole>();
            WatchLists = new HashSet<WatchList>();

        }

        public Guid Id { get; set; }
        public int? GenderId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Pronoun { get; set; }
        public string Email { get; set; }
        public DateTime Age { get; set; }
        public DateTime JoinDate { get; set; }
        public string PasswordHash { get; set; }
        public bool WantsNotifications { get; set; }
        public bool ConfirmedEmail { get; set; }
        public Guid? ImageId { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Image Image { get; set; }
        public virtual ICollection<ArtReview> ArtReviews { get; set; }
        public virtual ICollection<FollowList> FollowListFollowedUsers { get; set; }
        public virtual ICollection<FollowList> FollowListFollowerUsers { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<WatchList> WatchLists { get; set; }

    }
}
