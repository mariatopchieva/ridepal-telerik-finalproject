using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.DTO
{
    public class GeneratePlaylistDTO
    {
        public string StartLocation { get; set; }
        public string Destination { get; set; }
        public string PlaylistName { get; set; }
        public string FilePath { get; set; }
        public bool RepeatArtist { get; set; }
        public bool UseTopTracks { get; set; }
        public Dictionary<string, int> GenrePercentage { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}
