using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Models
{
    public class PlaylistGenre
    {
        public int Id { get; set; }
        public long GenreId { get; set; }
        public Genre Genre { get; set; }
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
