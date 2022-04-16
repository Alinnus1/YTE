using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class TvseriesGenre : IEntity
    {
        public TvseriesGenre()
        {
            TvseriesGenreTvseries = new HashSet<TvseriesGenreTvseries>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TvseriesGenreTvseries> TvseriesGenreTvseries { get; set; }
    }
}
