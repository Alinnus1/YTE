using AutoMapper;
using System;
using YTE.BusinessLogic.Implementation.ArtReview.Model;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.ArtReview.Mapping
{
    public class ArtReviewProfile : Profile
    {
        public ArtReviewProfile()
        {
            CreateMap<CreateArtReviewModel, Entities.ArtReview>();

            CreateMap<Entities.ArtReview, ListArtReviewModel>()
                .ForMember(a => a.UserName, a => a.MapFrom(s => s.User.UserName))
                .ForMember(a => a.Image, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.User.Image.Content)}"))
                .ForMember(a => a.ArtName, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.Route, a => a.MapFrom(s => Enum.GetName(typeof(ArtObjectTypes), s.ArtObject.TypeId))); ;

            CreateMap<Entities.ArtReview, ListUserArtReviewModel>()
                .ForMember(a => a.ArtName, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.UserName, a => a.MapFrom(s => s.User.UserName))
                .ForMember(a => a.Route, a => a.MapFrom(s => Enum.GetName(typeof(ArtObjectTypes), s.ArtObject.TypeId)));

            CreateMap<Entities.ArtReview, EditArtReviewModel>()
                .ForMember(a => a.ArtName, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.ArtObjectId, a => a.MapFrom(s => s.ArtObject.Id));

            CreateMap<Entities.ArtReview, ListUserProfileArtReviewModel>()
                .ForMember(a => a.ArtName, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.ArtObjectId, a => a.MapFrom(s => s.ArtObject.Id))
                .ForMember(a => a.UserName, a => a.MapFrom(s => s.User.UserName))
                .ForMember(a => a.Route, a => a.MapFrom(s => Enum.GetName(typeof(ArtObjectTypes), s.ArtObject.TypeId)));


        }
    }
}
