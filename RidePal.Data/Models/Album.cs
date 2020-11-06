using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models
{
    public class Album
    {
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string AlbumName { get; set; }

        [JsonPropertyName("genre_id")] //The album's first genre id (You should use the genre list instead). NB : -1 for not found
        public long? GenreId { get; set; }

        [JsonPropertyName("artist")]
        public Artist Artist { get; set; }

        public long? ArtistId { get; set; }


        [JsonPropertyName("tracks")]
        public List<Track> AlbumTracks { get; set; }

        [JsonPropertyName("link")]
        public string AlbumTrackListURL { get; set; }

        [JsonPropertyName("cover")]
        public string AlbumCoverURL { get; set; }

    }
}
