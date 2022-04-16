using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.Admin.Model;
using YTE.Entities;

namespace YTE.BusinessLogic.Implementation.Admin.Mapping
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<User,ListUserModel>()
                .ForMember(a => a.Image, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.Image.Content)}"));

            CreateMap<User, DetailsUserModel>()
                .ForMember(a => a.Gender, a => a.MapFrom(s => s.Gender.Name))
                .ForMember(a=>a.Roles, a => a.MapFrom(s=>s.UserRoles.Select(ur=>ur.Role.Name).ToList()))
                .ForMember(a => a.Age, a => a.MapFrom(s => GetAge(s.Age)))
                .ForMember(a => a.Image, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.Image.Content)}"));
            CreateMap<User, EditUserModel>()
                .ForMember(a => a.Image, opt => opt.Ignore());
        }
        public int GetAge(DateTime birth)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan nr = DateTime.Now - birth;
            return (zeroTime + nr).Year - 1;
        }
    }
}
