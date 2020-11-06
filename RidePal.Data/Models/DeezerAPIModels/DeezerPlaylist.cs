using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models.DeezerAPIModels
{
    public class DeezerPlaylist
    {
        [JsonPropertyName("title")]
        public string Name { get; set; }

        [JsonPropertyName("tracklist")]
        public string TracklistUrl { get; set; }

    }
}