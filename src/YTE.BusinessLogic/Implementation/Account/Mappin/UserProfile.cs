using AutoMapper;
using System;
using YTE.BusinessLogic.Implementation.Account.Model;
using YTE.Entities;

namespace YTE.BusinessLogic.Implementation.Account.Mappin
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel, User>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a=>a.Name, a=> a.MapFrom(s=> s.LastName))
                .ForMember(a=>a.Pronoun, a=> a.MapFrom(s=> s.FirstName))
                .ForMember(a => a.PasswordHash, a => a.MapFrom(s => s.Password));

            CreateMap<User, EditUserAccountModel>()
                .ForMember(a => a.Image, opt => opt.Ignore())
                .ForMember(a => a.LastName, a=> a.MapFrom(s=> s.Name))
                .ForMember(a => a.FirstName, a=> a.MapFrom(s=> s.Pronoun));

            CreateMap<EditUserAccountModel, User>()
                .ForMember(a => a.Name, a => a.MapFrom(s => s.LastName))
                .ForMember(a => a.Pronoun, a => a.MapFrom(s => s.FirstName))
                .ForMember(a => a.Image, opt => opt.Ignore());

            CreateMap<User, DetailsUserAccountModel>()
                .ForMember(a => a.Gender, a => a.MapFrom(s => s.Gender.Name))
                .ForMember(a => a.Age, a => a.MapFrom(s => GetAge(s.Age)))
                .ForMember(a => a.Image, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.Image.Content)}"));
        }
        public int GetAge(DateTime birth)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            TimeSpan nr = DateTime.Now - birth;
            return (zeroTime + nr).Year - 1;
        }
    }
}
