using System;

namespace YTE.BusinessLogic.Implementation.Film.Model
{
    public interface IFilmModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public TimeSpan Length { get; set; }
        public string Studio { get; set; }
    }
}
