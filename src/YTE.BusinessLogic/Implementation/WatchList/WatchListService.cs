using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.WatchList.Model;
using YTE.Common.DTOS;
using YTE.DataAccess;

namespace YTE.BusinessLogic.Implementation.WatchList
{
    public class WatchListService : BaseService
    {
        public WatchListService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public PaginatedList<ListWatchListModel> GetOf(string username, int pageNumber)
        {

            var watchListArtObjects = UnitOfWork.WatchLists.Get()
                    .Include(a => a.User)
                    .Include(a => a.ArtObject)
                        .ThenInclude(a => a.Poster)
                .Where(f => f.UserName == username)
                .Select(f => Mapper.Map<Entities.WatchList, ListWatchListModel>(f));

            var paginatedWatchListArtObjects = PaginatedList<ListWatchListModel>.Create(watchListArtObjects, pageNumber, 10);

            return paginatedWatchListArtObjects;
        }

        public void Add(Guid artid)
        {
            var username = CurrentUser.UserName;
            if (!CheckExperienced(username, artid))
            {
                ExecuteInTransaction(uow =>
                {

                    uow.WatchLists.Insert(new Entities.WatchList()
                    {
                        ArtObjectId = artid,
                        UserName = username
                    });
                    uow.SaveChanges();
                });
            }
        }

        public void Remove(Guid artid)
        {
            var username = CurrentUser.UserName;
            ExecuteInTransaction(uow =>
            {
                var fav = uow.WatchLists.Get()
                    .FirstOrDefault(a => a.ArtObjectId == artid && a.UserName == username);

                uow.WatchLists.Delete(fav);
                uow.SaveChanges();
            });
        }
        public void Remove(UnitOfWork uow, string username, Guid id)
        {
            if (uow.WatchLists.Get()
                    .Any(a => a.ArtObjectId == id && a.UserName == username))
            {
                var fav = uow.WatchLists.Get()
                        .FirstOrDefault(a => a.ArtObjectId == id && a.UserName == username);
                uow.WatchLists.Delete(fav);
            }
        }

        public bool CheckAdded(string username, Guid artid)
        {
            return UnitOfWork.WatchLists.Get()
                .Any(a => a.ArtObjectId == artid && a.UserName == username);
        }

        public bool CheckExperienced(string username, Guid id)
        {
            var userid = UnitOfWork.Users.Get()
                    .FirstOrDefault(a => a.UserName == username).Id;
            return UnitOfWork.ArtReview.Get()
                .Any(f => f.ArtObjectId == id && f.UserId == userid);
        }
    }
}
