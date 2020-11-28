using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RidePal.Service.DTO
{
    public class TrackDTO
    {
        public TrackDTO(Track track)
        {
            this.Id = track.Id;
            this.ArtistId = track.ArtistId;
            this.Artist = track.Artist;
            this.TrackTitle = track.TrackTitle;
            this.TrackDuration = track.TrackDuration;
            this.Album = track.Album;
            this.AlbumId = track.AlbumId;
            this.TrackRank = track.TrackRank;
            this.TrackUrl = track.TrackUrl;
            this.TrackPreviewURL = track.TrackPreviewURL;
            this.Genre = track.Genre;
            this.GenreId = track.GenreId;
            this.Playlists = track.Playlists;
        }

        public TrackDTO()
        {

        }

        public long Id { get; set; }

        public long? ArtistId { get; set; }

        public Artist Artist { get; set; }

        [DisplayName("Name")]
        public string TrackTitle { get; set; }

        [DisplayName("Duration")]
        public int TrackDuration { get; set; }
        public long? AlbumId { get; set; }

        public Album Album { get; set; }

        [DisplayName("Rank")]
        public long TrackRank { get; set; }
        
        public string TrackUrl { get; set; }

        public string TrackPreviewURL { get; set; }

        public long? GenreId { get; set; }
        public Genre Genre { get; set; }

        public virtual ICollection<PlaylistTrack> Playlists { get; set; }
    }
}
