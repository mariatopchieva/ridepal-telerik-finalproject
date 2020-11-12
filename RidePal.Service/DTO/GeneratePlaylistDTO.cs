using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.DTO
{
    public class GeneratePlaylistDTO
    {
        public string StartLocationName { get; set; }
        public string DestinationName { get; set; }
        public string PlaylistName { get; set; }
        public bool RepeatArtist { get; set; }
        public bool UseTopTracks { get; set; }
        public Dictionary<string, int> GenrePercentage { get; set; }
        public User User { get; set; }
    }
}
