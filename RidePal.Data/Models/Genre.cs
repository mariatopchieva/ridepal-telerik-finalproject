using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Data.Models
{
    public class Genre
    {
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public virtual ICollection<PlaylistGenre> Playlists { get; set; }

        [JsonPropertyName("picture")]
        public string GenrePictureURL { get; set; }
    }
}
