using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.FavoriteList.Model;
using YTE.Common.DTOS;
using YTE.DataAccess;

namespace YTE.BusinessLogic.Implementation.FavoriteList
{
    public class FavoriteListService : BaseService
    {
        public FavoriteListService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }
        public PaginatedList<ListFavoriteListModel> GetOf(string username, int pageNumber)
        {
            var userid = UnitOfWork.Users.Get()
                            .Where(u => u.UserName == username)
                            .Select(u => u.Id)
                            .FirstOrDefault();

            var favList = UnitOfWork.FavoriteLists.Get()
                .Include(a => a.ArtReview)
                    .ThenInclude(a => a.ArtObject)
                        .ThenInclude(a => a.Poster)
                        
                .Where(f => f.UserId == userid)
                .Select(f => Mapper.Map<Entities.FavoriteList, ListFavoriteListModel>(f));

            var paginatedFavList = PaginatedList<ListFavoriteListModel>.Create(favList, pageNumber, 10);

            return paginatedFavList;

        }

        public void Add(Guid userid, Guid id)
        {
            if (CheckExperienced(userid, id))
            {
                ExecuteInTransaction(uow =>
                {
                    
                    uow.FavoriteLists.Insert(new Entities.FavoriteList()
                    {
                        ArtObjectId = id,
                        UserId = userid
                    });
                    uow.SaveChanges();
                });
            }
        }
        public void Remove( Guid id)
        {
            var userid = CurrentUser.Id;
            ExecuteInTransaction(uow =>
            {
                var fav = uow.FavoriteLists.Get()
                    .FirstOrDefault(a => a.ArtObjectId == id && a.UserId == userid);

                uow.FavoriteLists.Delete(fav);
                uow.SaveChanges();
            });
        }

        

        public bool CheckExperienced(Guid userid, Guid id)
        {
            return UnitOfWork.ArtReview.Get()
                .Any(f => f.ArtObjectId == id && f.UserId == userid);
        }
        public bool CheckAdded(Guid userid, Guid id)
        {
            return UnitOfWork.FavoriteLists.Get()
                .Any(a => a.ArtObjectId == id && a.UserId == userid);
        }
    }
}
