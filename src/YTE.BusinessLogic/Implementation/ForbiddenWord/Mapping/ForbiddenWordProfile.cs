using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.BusinessLogic.Implementation.ForbiddenWord.Model;

namespace YTE.BusinessLogic.Implementation.ForbiddenWord.Mapping
{
    public class ForbiddenWordProfile : Profile
    {
        public ForbiddenWordProfile()
        {
            CreateMap<CreateForbiddenWordModel, Entities.ForbiddenWord>();
            CreateMap<Entities.ForbiddenWord,CreateForbiddenWordModel>();
            CreateMap<Entities.ForbiddenWord, ListForbiddenWordModel>();



        }
    }
}
