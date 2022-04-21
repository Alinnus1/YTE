using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace YTE.BusinessLogic.Implementation.Manga.Model
{
    public class CreateMangaModel : IMangaModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public int NoVolumes { get; set; }
        public int NoChapters { get; set; }
        public bool IsFinished { get; set; }
        public SelectList GenreList { get; set; }
        public List<int> selectedGenres { get; set; }
        public Guid? PosterId { get; set; }
        public byte[] PosterContent { get; set; }
        public Guid? BackgroundId { get; set; }
        public byte[] BackgroundContent { get; set; }
    }
}
