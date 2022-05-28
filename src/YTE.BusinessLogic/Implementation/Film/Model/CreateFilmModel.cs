using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace YTE.BusinessLogic.Implementation.Film.Model
{
    public class CreateFilmModel : IFilmModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public TimeSpan Length { get; set; }
        public string Studio { get; set; }
        public SelectList GenreList { get; set; }
        public List<int> selectedGenres { get; set; }
    }
}
