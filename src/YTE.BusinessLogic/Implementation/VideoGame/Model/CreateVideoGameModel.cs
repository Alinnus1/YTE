using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.VideoGame.Model
{
    public class CreateVideoGameModel : IVideoGameModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string Esrbrating { get; set; }
        public bool IsMultiplayer { get; set; }
        public SelectList GenreList { get; set; }
        public List<int> selectedGenres { get; set; }


    }

}
