using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models.DeezerAPIModels
{
    public class DeezerTrackCollection
    {
        [JsonPropertyName("data")]
        public IEnumerable<Track> Tracks { get; set; }

        [JsonPropertyName("next")]
        public string NextPageUrl { get; set; }

    }
}