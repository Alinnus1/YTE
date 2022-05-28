using AutoMapper;
using System;
using YTE.BusinessLogic.Implementation.Film.Model;

namespace YTE.BusinessLogic.Implementation.Account.Mapping
{
    public class FilmProfile : Profile
    {
        public FilmProfile()
        {
            CreateMap<CreateFilmModel, Entities.ArtObject>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.IsDeleted, a => a.MapFrom(s => false));

            CreateMap<CreateFilmModel, Entities.Film>();

            CreateMap<Entities.Film, ListFilmModel>()
                .ForMember(a => a.Name, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.Author, a => a.MapFrom(s => s.ArtObject.Author))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(s => s.ArtObject.ReleaseDate))
                .ForMember(a => a.Poster, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.ArtObject.Poster.Content)}"));

            CreateMap<Entities.Film, DetailsFilmModel>()
                .ForMember(a => a.Name, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.Author, a => a.MapFrom(s => s.ArtObject.Author))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(s => s.ArtObject.ReleaseDate.Date))
                .ForMember(a => a.Description, a => a.MapFrom(s => s.ArtObject.Description))
                .ForMember(a => a.Language, a => a.MapFrom(s => s.ArtObject.Language))
                .ForMember(a => a.Poster, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.ArtObject.Poster.Content)}"))
                .ForMember(a => a.Background, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.ArtObject.Background.Content)}"));

            CreateMap<Entities.Film, EditFilmModel>()
                .ForMember(a => a.Name, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.Author, a => a.MapFrom(s => s.ArtObject.Author))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(s => s.ArtObject.ReleaseDate.Date))
                .ForMember(a => a.Description, a => a.MapFrom(s => s.ArtObject.Description))
                .ForMember(a => a.Language, a => a.MapFrom(s => s.ArtObject.Language));

            CreateMap<EditFilmModel, Entities.Film>();

            CreateMap<DetailsFilmModel, Entities.Film>();

        }
    }
}
