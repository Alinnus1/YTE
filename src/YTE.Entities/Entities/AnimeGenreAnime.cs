﻿using System;
using System.Collections.Generic;
using YTE.Common;

#nullable disable

namespace YTE.Entities
{
    public partial class AnimeGenreAnime 
    {
        public int GenreId { get; set; }
        public Guid AnimeId { get; set; }

        public virtual Anime Anime { get; set; }
        public virtual AnimeGenre Genre { get; set; }
    }
}