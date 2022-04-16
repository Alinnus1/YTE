using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.FollowList.Model;
using YTE.BusinessLogic.Implementation.MailSender;
using YTE.Common.DTOS;

namespace YTE.BusinessLogic.Implementation.FollowList
{
    public class FollowListService : BaseService
    {
        private readonly MailSenderService mailSenderService;
        public FollowListService(ServiceDependencies serviceDependencies,MailSenderService mailSender) : base(serviceDependencies)
        {
            this.mailSenderService = mailSender;
        }

        // get users that current follows
        public PaginatedList<ListFollowersListModel> GetUserFollowings(string username,string searchString, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "";
            }
            var followes = UnitOfWork.FollowLists.Get()
                    .Include(a => a.FollowerUser)
                    .Include(a => a.FollowedUser)
                        .ThenInclude(u => u.Image)
                    .Where(a => a.FollowerUser.UserName == username && a.FollowedUser.UserName.Contains(searchString))
                    .Select(a => Mapper.Map<Entities.FollowList, ListFollowersListModel>(a));
            var paginatedFollowes = PaginatedList<ListFollowersListModel>.Create(followes, pageNumber, 25);

            return paginatedFollowes;


        }

        // get current's followers
        public List<ListFollowersListModel> GetFollowersOf(string username, string searchString, int pageNumber)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = "";
            }
            var followers = UnitOfWork.FollowLists.Get()
                    .Include(a => a.FollowedUser)
                    .Include(a => a.FollowerUser)
                        .ThenInclude(a => a.Image)
                    .Where(a => a.FollowedUser.UserName == username && a.FollowerUser.UserName.Contains(searchString))
                    .Select(a => Mapper.Map<Entities.FollowList, ListFollowersListModel>(a));
            var paginatedFollowers = PaginatedList<ListFollowersListModel>.Create(followers, pageNumber, 25);

            return paginatedFollowers;
        }

        public string GetFollowerUserName(string username)
        {
            return UnitOfWork.Users.Get()
                    .FirstOrDefault(u => u.UserName == username)?.UserName;
        }

        public void Add( Guid FollowedId)
        {
            var FollowerId = CurrentUser.Id;
            if (!CheckFollowing(FollowerId, FollowedId))
            {
                ExecuteInTransaction(uow =>
                {
                    uow.FollowLists.Insert(new Entities.FollowList()
                    {
                        FollowerUserId = FollowerId,
                        FollowedUserId = FollowedId,
                        Date = DateTime.Now
                    });
                    uow.SaveChanges();
                });
                mailSenderService.SendFollowNotification(FollowerId, FollowedId);
            }

        }

        public void Remove( Guid FollowedId)
        {
            var FollowerId = CurrentUser.Id;
            if (CheckFollowing(FollowerId, FollowedId))
            {
                ExecuteInTransaction(uow => 
                {
                    var x = uow.FollowLists.Get()
                        .FirstOrDefault(f => f.FollowerUserId == FollowerId && f.FollowedUserId == FollowedId);
                    uow.FollowLists.Delete(x);
                    uow.SaveChanges();
                });

                mailSenderService.SendUnFollowNotification(FollowerId, FollowedId);
            }
        }

        public bool CheckFollowing(Guid FollowerId, Guid FollowedId)
        {
            return UnitOfWork.FollowLists.Get()
                .Where(f => f.FollowerUserId == FollowerId && f.FollowedUserId == FollowedId)
                .Any();
        }

        public void SendFollowingLogicNotifications()
        {
            var arrangement = UnitOfWork.FollowLists.Get()
                .AsEnumerable()
                .GroupBy(u => u.FollowerUserId)
                ;

            foreach (var item in arrangement)
            {
                var follower = UnitOfWork.Users.Get()
                        .FirstOrDefault(u => u.Id == item.Key);
                var message = new StringBuilder($"Hello {follower.UserName}. Lets see what the people that you are following have reviewed in the past week! </br>");

                foreach (var followedId in item.Select(i=>i.FollowedUserId))
                {
                    var dt = DateTime.Now.AddDays(-7);
                    var followed = UnitOfWork.Users.Get()
                        .FirstOrDefault(u => u.Id == followedId);
                    var reviews = UnitOfWork.ArtReview.Get()
                        .Include(a=>a.ArtObject)
                        .Where(a => a.UserId == followed.Id && a.Date > dt);
                    if (reviews.Count() == 0)
                    {
                        message.AppendLine($"{followed.UserName} has not published any reviews this week!");
                    }else
                    {
                        message.AppendLine($"{followed.UserName} has published the following reviews:") ;
                        foreach (var review in reviews)
                        {
                            message.AppendLine( $"For {review.ArtObject.Name}, he gave {review.Score} and his thoughts were <i>'{review.Review}'</i>.");
                        }

                    }
                }
                mailSenderService.SendFollowingLogicEmail(follower.Email, message.ToString());

            }

          


        }

        public int GetNumbersOfFollowersOf(Guid userId)
        {
            return UnitOfWork.FollowLists.Get()
                .Where(f => f.FollowedUserId == userId)
                .Count();

        }
    }
}
