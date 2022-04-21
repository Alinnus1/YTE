using AutoMapper;
using YTE.BusinessLogic.Implementation.Genre.Model;
using YTE.Entities.Enums;

namespace YTE.BusinessLogic.Implementation.Genre.Mapping
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<CreateGenreModel, Entities.MangaGenre>();
            CreateMap<CreateGenreModel, Entities.FilmGenre>();
            CreateMap<CreateGenreModel, Entities.VideoGameGenre>();

            CreateMap<Entities.MangaGenre, ListGenreModel>()
                .ForMember(a => a.GenreType, a => a.MapFrom(s => GenreType.MangaGenre));
            CreateMap<Entities.FilmGenre, ListGenreModel>()
               .ForMember(a => a.GenreType, a => a.MapFrom(s => GenreType.FilmGenre));
            CreateMap<Entities.VideoGameGenre, ListGenreModel>()
               .ForMember(a => a.GenreType, a => a.MapFrom(s => GenreType.VideoGameGenre));
        }
    }
}
