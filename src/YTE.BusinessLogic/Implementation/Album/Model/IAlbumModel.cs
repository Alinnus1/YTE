using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Album.Model
{
    public interface IAlbumModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public TimeSpan Length { get; set; }
        public int NoTracks { get; set; }
    }
}
