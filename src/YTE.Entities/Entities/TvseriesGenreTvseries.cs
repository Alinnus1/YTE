using System;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class TvseriesGenreTvseries : IEntity
    {
        public int GenreId { get; set; }
        public Guid TvseriesId { get; set; }

        public virtual TvseriesGenre Genre { get; set; }
        public virtual Tvseries Tvseries { get; set; }
    }
}
