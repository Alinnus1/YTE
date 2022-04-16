using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.FavoriteList.Model;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.FavoriteList.Mapping
{
    public class FavoriteList : Profile
    {
        public FavoriteList()
        {
            CreateMap<Entities.FavoriteList, ListFavoriteListModel>()
                .ForMember(a => a.ArtName, a => a.MapFrom(s => s.ArtReview.ArtObject.Name
                   ))
                .ForMember(a => a.Poster, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.ArtReview.ArtObject.Poster.Content)}"))
                .ForMember(a => a.Review, a => a.MapFrom(s => s.ArtReview.Review))
                .ForMember(a => a.Score, a => a.MapFrom(s => s.ArtReview.Score))
                .ForMember(a => a.Date, a => a.MapFrom(s => s.ArtReview.Date))
                .ForMember(a => a.ExperiencedDate, a => a.MapFrom(s => s.ArtReview.ExperiencedAt))
                .ForMember(a=>a.Route,a=>a.MapFrom(s=>Enum.GetName(typeof(ArtObjectTypes),s.ArtReview.ArtObject.TypeId)));
        }
    }
}
