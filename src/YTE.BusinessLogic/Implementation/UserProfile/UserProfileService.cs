using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ArtReview;
using YTE.BusinessLogic.Implementation.FollowList;
using YTE.BusinessLogic.Implementation.UserProfile.Model;
using YTE.Common.DTOS;
using YTE.Common.Exceptions;
using YTE.Entities;

namespace YTE.BusinessLogic.Implementation.UserProfile
{
    public class UserProfileService : BaseService
    {
        private readonly ArtReviewService ReviewService;
        private readonly FollowListService FollowService;

        public UserProfileService(ServiceDependencies serviceDependencies, ArtReviewService reviewService, FollowListService followService) : base(serviceDependencies)
        {
            this.ReviewService = reviewService;
            this.FollowService = followService;
        }

        public List<ListUserProfileModel> GetUserProfiles(int pageNumber)
        {
            var profiles = UnitOfWork.Users.Get()
                .Include(a => a.ArtReviews)
                .Include(a => a.Image)
                .Select(a => Mapper.Map<User, ListUserProfileModel>(a));

            var paginatedProfiles = PaginatedList<ListUserProfileModel>.Create(profiles, pageNumber, 10);
            paginatedProfiles.ForEach(x => x.Followed = FollowService.CheckFollowing(CurrentUser.Id, x.Id));

            return paginatedProfiles;
        }

        public List<ListUserProfileModel> GetUserProfilesFilter(string searchString, int pageNumber)
        {
            var profiles = UnitOfWork.Users.Get()
                .Include(a => a.ArtReviews)
                .Include(a => a.Image)
                .Where(a => a.UserName.Contains(searchString))
                .Select(a => Mapper.Map<User, ListUserProfileModel>(a))
                ;
            var paginatedProfiles = PaginatedList<ListUserProfileModel>.Create(profiles, pageNumber, 5);
            paginatedProfiles.ForEach(x => x.Followed = FollowService.CheckFollowing(CurrentUser.Id, x.Id));

            return paginatedProfiles;
        }

        public DetailsUserProfileModel GetUserProfile(string id)
        {
            var user = UnitOfWork.Users.Get()
                .Where(u => u.UserName == id)
                .Include(a => a.ArtReviews)
                .Include(a => a.Image)
                .Select(a => Mapper.Map<User, DetailsUserProfileModel>(a))
                .FirstOrDefault();

            if (user == null)
            {
                throw new NotFoundErrorException("User Not Found");
            }

            user.MostNegativeReviews = ReviewService.GetSpecificUserReviewsOfArtForProfile(id, "Score", false);
            user.MostPositiveReviews = ReviewService.GetSpecificUserReviewsOfArtForProfile(id, "Score", true);
            user.RecentReviews = ReviewService.GetSpecificUserReviewsOfArtForProfile(id, "Date", true);
            user.Followed = FollowService.CheckFollowing(CurrentUser.Id, user.Id);
            user.NoFollowers = FollowService.GetNumbersOfFollowersOf(user.Id);
            return user;
        }
    }
}
