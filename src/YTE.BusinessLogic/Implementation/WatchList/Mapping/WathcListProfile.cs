using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.WatchList.Model;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.WatchList.Mapping
{
    public class WathcListProfile : Profile
    {
        public WathcListProfile()
        {
            CreateMap<Entities.WatchList, ListWatchListModel>()
                .ForMember(a => a.ArtName, a => a.MapFrom(s => s.ArtObject.Name
                   ))
                .ForMember(a => a.Poster, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.ArtObject.Poster.Content)}"))
                .ForMember(a => a.Route, a => a.MapFrom(s => Enum.GetName(typeof(ArtObjectTypes), s.ArtObject.TypeId)));
        }
    }
}
