using System;
using System.Collections.Generic;
using YTE.BusinessLogic.Implementation.ArtReview.Model;

namespace YTE.BusinessLogic.Implementation.VideoGame.Model
{
    public class DetailsVideoGameModel : IVideoGameModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string Esrbrating { get; set; }
        public bool IsMultiplayer { get; set; }
        public List<string> GenreList { get; set; }
        public string Background { get; set; }
        public string Poster { get; set; }
        public bool IsReviewedByCurrentUser { get; set; }
        public bool AddedToFavoriteList { get; set; }
        public bool EligibleFavoriteList { get; set; }
        public bool AddedToWatchList { get; set; }
        public decimal Average { get; set; }
        public int NoReviews { get; set; }

        public List<ListArtReviewModel> RecentReviews { get; set; }
        public List<ListArtReviewModel> MostPositiveReviews { get; set; }
        public List<ListArtReviewModel> MostNegativeReviews { get; set; }
    }

}

