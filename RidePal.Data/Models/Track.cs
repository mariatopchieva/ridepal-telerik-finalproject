using RidePal.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models
{
    public class Track : Entity
    {
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public long? ArtistId { get; set; }

        [JsonPropertyName("artist")]
        public Artist Artist { get; set; }

        [JsonPropertyName("title")]
        public string TrackTitle { get; set; }

        [JsonPropertyName("duration")] //in seconds
        public int TrackDuration { get; set; }
        public long? AlbumId { get; set; }

        [JsonPropertyName("album")]
        public Album Album { get; set; }

        [JsonPropertyName("rank")]
        public long TrackRank { get; set; }

        [JsonPropertyName("link")]
        public string TrackUrl { get; set; }

        [JsonPropertyName("preview")]
        public string TrackPreviewURL { get; set; }

        public long? GenreId { get; set; }
        public Genre Genre { get; set; }

        public virtual ICollection<PlaylistTrack> Playlists { get; set; }
    }
}
