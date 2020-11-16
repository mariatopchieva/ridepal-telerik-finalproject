using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.DTO
{
    public class PlaylistDTO
    {
        public PlaylistDTO(Playlist playlist) //playlist=> playlistDTO
        {
            this.Id = playlist.Id;
            this.UserId = playlist.UserId;
            this.Title = playlist.Title;
            this.PlaylistPlaytime = playlist.PlaylistPlaytime;
            this.Rank = playlist.Rank;
            this.FilePath = playlist.FilePath;
        }

        public PlaylistDTO()
        {
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public double PlaylistPlaytime { get; set; }

        public double Rank { get; set; }

        public string FilePath { get; set; }

    }
}
