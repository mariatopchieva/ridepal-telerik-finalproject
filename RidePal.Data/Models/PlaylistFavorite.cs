using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class PlaylistFavorite
    {
        public PlaylistFavorite(int userId, int playlistId, bool isFavorate)
        {
            this.UserId = userId;
            this.PlaylistId = playlistId;
            this.IsFavorite = isFavorate;
        }

        public PlaylistFavorite()
        {
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }

        public User User { get; set; }
        [Required]
        public int PlaylistId { get; set; }

        public Playlist Playlist { get; set; }

        public bool? IsFavorite { get; set; }
    }
}
