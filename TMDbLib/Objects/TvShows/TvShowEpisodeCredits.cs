﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace TMDbLib.Objects.TvShows
{
    public class TvShowEpisodeCredits : Credits
    {
        [JsonProperty("guest_stars")]
        public List<Cast> GuestStars { get; set; }
    }
}