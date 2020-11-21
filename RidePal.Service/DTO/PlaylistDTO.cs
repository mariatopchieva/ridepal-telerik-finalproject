using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RidePal.Service.DTO
{
    public class PlaylistDTO
    {
        public PlaylistDTO(Playlist playlist) //playlist=> playlistDTO
        {
            this.Id = playlist.Id;
            this.UserId = playlist.UserId;
            this.User = playlist.User;
            this.Title = playlist.Title;
            this.PlaylistPlaytime = playlist.PlaylistPlaytime;
            this.Rank = playlist.Rank;
            this.PlaytimeString = playlist.PlaytimeString;
            this.Tracks = playlist.Tracks;
            this.TracksCount = playlist.TracksCount;
            this.Genres = playlist.Genres;
            this.GenresCount = playlist.GenresCount;
            this.Favorites = playlist.Favorites;
            this.FilePath = playlist.FilePath;
        }

        public PlaylistDTO()
        {

        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string Title { get; set; }

        [DisplayName("Duration in seconds")]
        public double PlaylistPlaytime { get; set; }

        [DisplayName("Duration")]
        public string PlaytimeString { get; set; }

        public int Rank { get; set; }

        [DisplayName("Image")]
        public string FilePath { get; set; }
        public string StartLocation { get; set; }
        public string Destination { get; set; }

        public ICollection<PlaylistTrack> Tracks { get; set; }

        public ICollection<PlaylistGenre> Genres { get; set; }

        public ICollection<PlaylistFavorite> Favorites { get; set; }

        [DisplayName("Number of tracks")]
        public int TracksCount { get; set; }

        [DisplayName("Number of genres")]
        public int GenresCount { get; set; }
    }
}
