using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Base;
using YTE.BusinessLogic.Implementation.ArtObject.Model;
using YTE.BusinessLogic.Implementation.Film.Model;
using YTE.Common;
using YTE.Common.DTOS;
using YTE.DataAccess;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.ArtObject
{
    public class ArtObjectService : BaseService
    {
        public ArtObjectService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {

        }

        public void CreateNewArtObject(CreateArtObjectModel model)
        {
            ExecuteInTransaction(uow =>
            {
                var artObject = Mapper.Map<CreateArtObjectModel, Entities.ArtObject>(model);

                uow.ArtObjects.Insert(artObject);

                uow.SaveChanges();
            });
        }


        public List<CreateArtObjectModel> GetArtObjects()
        {
            return UnitOfWork.ArtObjects.Get()
                .Select(a => Mapper.Map<Entities.ArtObject, CreateArtObjectModel>(a))
                .ToList();
        }
        public Entities.ArtObject EditArtObject(IEditArtModel model, UnitOfWork uow)
        {
            var artObject = uow.ArtObjects.Get()
                                .Include(a => a.Poster)
                                .Include(a => a.Background)
                                .FirstOrDefault(a => a.Id == model.Id);

            artObject.Name = model.Name;
            artObject.Author = model.Author;
            artObject.ReleaseDate = model.ReleaseDate;
            artObject.Language = model.Language;
            artObject.Description = model.Description;
            return artObject;
        }


    }
}
