using AutoMapper;
using System;
using YTE.BusinessLogic.Implementation.Album.Model;

namespace YTE.BusinessLogic.Implementation.Album.Mapping
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile()
        {
            CreateMap<CreateAlbumModel, Entities.ArtObject>()
                .ForMember(a => a.Id, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.IsDeleted, a => a.MapFrom(s => false));

            CreateMap<CreateAlbumModel, Entities.Album>();

            CreateMap<Entities.Album, ListAlbumModel>()
                .ForMember(a => a.Name, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.Author, a => a.MapFrom(s => s.ArtObject.Author))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(s => s.ArtObject.ReleaseDate))
                .ForMember(a => a.Poster, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.ArtObject.Poster.Content)}"));

            CreateMap<Entities.Album, DetailsAlbumModel>()
                .ForMember(a => a.Name, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.Author, a => a.MapFrom(s => s.ArtObject.Author))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(s => s.ArtObject.ReleaseDate.Date))
                .ForMember(a => a.Description, a => a.MapFrom(s => s.ArtObject.Description))
                .ForMember(a => a.Language, a => a.MapFrom(s => s.ArtObject.Language))
                .ForMember(a => a.Poster, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.ArtObject.Poster.Content)}"))
                .ForMember(a => a.Background, a => a.MapFrom(s => $"data:image/gif;base64,{Convert.ToBase64String(s.ArtObject.Background.Content)}"));

            CreateMap<Entities.Album, EditAlbumModel>()
                .ForMember(a => a.Name, a => a.MapFrom(s => s.ArtObject.Name))
                .ForMember(a => a.Author, a => a.MapFrom(s => s.ArtObject.Author))
                .ForMember(a => a.ReleaseDate, a => a.MapFrom(s => s.ArtObject.ReleaseDate.Date))
                .ForMember(a => a.Description, a => a.MapFrom(s => s.ArtObject.Description))
                .ForMember(a => a.Language, a => a.MapFrom(s => s.ArtObject.Language));

            CreateMap<EditAlbumModel, Entities.Album>();

            CreateMap<DetailsAlbumModel, Entities.Album>();

        }
    }
}
