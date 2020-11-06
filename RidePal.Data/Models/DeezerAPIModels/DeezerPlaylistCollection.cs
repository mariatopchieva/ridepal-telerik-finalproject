using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models.DeezerAPIModels
{
    public class DeezerPlaylistCollection
    {
        [JsonPropertyName("data")]
        public IEnumerable<DeezerPlaylist> DeezerPlaylists { get; set; }

        [JsonPropertyName("next")]
        public string NextPageUrl { get; set; }
    }
}
