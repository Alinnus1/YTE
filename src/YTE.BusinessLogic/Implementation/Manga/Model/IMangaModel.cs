using System;

namespace YTE.BusinessLogic.Implementation.Manga.Model
{
    public interface IMangaModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public int NoVolumes { get; set; }
        public int NoChapters { get; set; }
        public bool IsFinished { get; set; }
    }
}
