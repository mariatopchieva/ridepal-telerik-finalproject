using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Models
{
    public class PlaylistTrack
    {
        public int Id { get; set; }
        public long TrackId { get; set; }
        public Track Track { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
