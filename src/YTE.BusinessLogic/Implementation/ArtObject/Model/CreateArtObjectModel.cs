using System;

namespace YTE.BusinessLogic.Implementation.ArtObject.Model
{
    public class CreateArtObjectModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
    }
}
