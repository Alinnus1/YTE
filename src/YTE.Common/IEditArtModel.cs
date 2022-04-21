using Microsoft.AspNetCore.Http;
using System;

namespace YTE.Common
{
    public interface IEditArtModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public IFormFile Poster { get; set; }
        public IFormFile Background { get; set; }
    }
}
