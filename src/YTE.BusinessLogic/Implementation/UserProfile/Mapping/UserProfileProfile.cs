using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.UserProfile.Model;
using YTE.Entities;

namespace YTE.BusinessLogic.Implementation.UserProfile.Mapping
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<User, ListUserProfileModel>()
                .ForMember(a => a.Image, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.Image.Content)}"))
                .ForMember(a => a.NoReviews, a => a.MapFrom(s => s.ArtReviews.Count));

            CreateMap<User,DetailsUserProfileModel>()
                .ForMember(a => a.Image, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.Image.Content)}"))
                .ForMember(a => a.Age, a => a.MapFrom(s => GetAge(s.Age)))
                .ForMember(a => a.NoReviews, a => a.MapFrom(s => s.ArtReviews.Count));

        }
        public int GetAge(DateTime birth)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan nr = DateTime.Now - birth;
            return (zeroTime + nr).Year - 1;
        }
    }
}
