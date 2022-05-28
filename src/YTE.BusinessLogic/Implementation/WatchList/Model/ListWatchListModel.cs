using System;

namespace YTE.BusinessLogic.Implementation.WatchList.Model
{
    public class ListWatchListModel
    {
        public Guid UserId { get; set; }
        public Guid ArtObjectId { get; set; }
        public string ArtName { get; set; }
        public string Poster { get; set; }
        public string Route { get; set; }
    }
}
