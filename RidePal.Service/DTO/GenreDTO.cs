using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.DTO
{
    public class GenreDTO
    {
        public GenreDTO(Genre genre)
        {
            this.Id = genre.Id;
            this.Name = genre.Name;
            this.GenrePictureURL = genre.GenrePictureURL;
            this.Playlists = genre.Playlists;
        }

        public GenreDTO()
        {
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PlaylistGenre> Playlists { get; set; }

        public string GenrePictureURL { get; set; }

    }
}
