using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTE.Common;

namespace YTE.BusinessLogic.Implementation.Album.Model
{
    public class EditAlbumModel : IAlbumModel, IEditArtModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public TimeSpan Length { get; set; }
        public int NoTracks{ get; set; }
        public IFormFile Poster { get; set; }
        public IFormFile Background { get; set; }
        public List<int> selectedGenres { get; set; }
    }
}
