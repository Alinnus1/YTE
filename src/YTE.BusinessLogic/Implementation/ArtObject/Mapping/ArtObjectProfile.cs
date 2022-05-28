using AutoMapper;
using System;
using YTE.BusinessLogic.Implementation.ArtObject.Model;

namespace YTE.BusinessLogic.Implementation.Account.Mapping
{
    public class ArtObjectProfile : Profile
    {
        public ArtObjectProfile()
        {
            CreateMap<CreateArtObjectModel, Entities.ArtObject>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.IsDeleted, a => a.MapFrom(s => false));

            CreateMap<Entities.ArtObject, CreateArtObjectModel>();
        }
    }
}
