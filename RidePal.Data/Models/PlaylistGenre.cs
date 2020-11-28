using RidePal.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class PlaylistGenre : Entity
    {

        public PlaylistGenre(long genreId, int playlistId)
        {
            this.GenreId = genreId;
            this.PlaylistId = playlistId;
        }
        
        [Key]
        public int Id { get; set; }
        [Required]
        public long GenreId { get; set; }
        public Genre Genre { get; set; }
        [Required]
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
