using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class PlaylistTrack
    {
        public PlaylistTrack(long trackId, int playlistId)
        {
            this.PlaylistId = playlistId;
            this.TrackId = trackId;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public long TrackId { get; set; }
        public Track Track { get; set; }
        [Required]
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
