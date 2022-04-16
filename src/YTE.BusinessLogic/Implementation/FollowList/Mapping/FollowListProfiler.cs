using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.FollowList.Model;

namespace YTE.BusinessLogic.Implementation.FollowList.Mapping
{
    public class FollowListProfiler : Profile
    {
        public FollowListProfiler()
        {
            CreateMap<Entities.FollowList, ListFollowersListModel>()
                
                .ForMember(a => a.FollowedUserName, a => a.MapFrom(s => s.FollowedUser.UserName))
                .ForMember(a => a.FollowerUserName, a => a.MapFrom(s => s.FollowerUser.UserName))
                .ForMember(a => a.FollowedImage, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.FollowedUser.Image.Content)}"))
                .ForMember(a => a.FollowerImage, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.FollowerUser.Image.Content)}"));
        }
    }
}
