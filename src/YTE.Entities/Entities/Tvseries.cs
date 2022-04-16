using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class Tvseries : IEntity
    {
        public Tvseries()
        {
            TvseriesGenreTvseries = new HashSet<TvseriesGenreTvseries>();
        }

        public Guid Id { get; set; }
        public TimeSpan AverageEpTime { get; set; }
        public int NoEpisodes { get; set; }
        public int NoSeasons { get; set; }
        public bool IsFinished { get; set; }

        public virtual ArtObject ArtObject { get; set; }
        public virtual ICollection<TvseriesGenreTvseries> TvseriesGenreTvseries { get; set; }
    }
}
