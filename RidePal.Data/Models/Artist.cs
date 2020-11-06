using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models
{
    public class Artist
    {
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string ArtistName { get; set; }
        public ICollection<Track> ArtistTracks { get; set; }

        [JsonPropertyName("tracklist")]
        public string ArtistTracklistURL { get; set; }

        [JsonPropertyName("picture")]
        public string ArtistPictureURL { get; set; }
    }
}
