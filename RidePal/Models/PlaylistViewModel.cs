using RidePal.Data.Models;
using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Models
{
    public class PlaylistViewModel
    {
        public PlaylistViewModel(PlaylistDTO playlistDTO)
        {
            this.Id = playlistDTO.Id;
            this.Title = playlistDTO.Title;
            this.Rank = playlistDTO.Rank;
            this.PlaylistPlaytime = playlistDTO.PlaylistPlaytime;
            this.User = playlistDTO.User;
            this.PlaytimeString = playlistDTO.PlaytimeString;
            this.FilePath = playlistDTO.FilePath;
            this.StartLocation = playlistDTO.StartLocation;
            this.Destination = playlistDTO.Destination;
            this.Tracks = playlistDTO.Tracks;
            this.Genres = playlistDTO.Genres;
            this.Favorites = playlistDTO.Favorites;
            this.TracksCount = playlistDTO.TracksCount;
            this.GenresCount = playlistDTO.GenresCount;
        }

        public PlaylistViewModel()
        {
        }

        [Key]
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

        public string Destination { get; set; }

        public string StartLocation { get; set; }

        public ICollection<PlaylistTrack> Tracks { get; set; }

        public ICollection<PlaylistGenre> Genres { get; set; }

        public ICollection<PlaylistFavorite> Favorites { get; set; }

        [DisplayName("Number of tracks")]
        public int TracksCount { get; set; }

        [DisplayName("Number of genres")]
        public int GenresCount { get; set; }
    }
}
