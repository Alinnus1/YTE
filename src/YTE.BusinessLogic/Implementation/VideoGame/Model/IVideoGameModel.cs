using System;

namespace YTE.BusinessLogic.Implementation.VideoGame.Model
{
    public interface IVideoGameModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string Esrbrating { get; set; }
        public bool IsMultiplayer { get; set; }
    }
}
