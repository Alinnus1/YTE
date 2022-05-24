using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTE.BusinessLogic.Implementation.Anime.Model
{
    public interface IAnimeModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public TimeSpan AverageEpTime { get; set; }
        public string Studio { get; set; }
        public int NoSeasons { get; set; }
        public int NoEpisodes { get; set; }
        public bool IsFinished { get; set; }

    }
}
