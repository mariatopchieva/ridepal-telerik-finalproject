using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.DTO
{
    public class GeneratePlaylistDTO
    {
        public string startLocationName { get; set; }
        public string destinationName { get; set; }
        public string playlistName { get; set; }
        public bool repeatArtist { get; set; }
        public bool useTopTracks { get; set; }
        public Dictionary<string, int> genrePercentage { get; set; }
        public User user { get; set; }
    }
}
