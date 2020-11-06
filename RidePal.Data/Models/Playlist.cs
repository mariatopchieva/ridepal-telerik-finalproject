﻿using RidePal.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class Playlist : Entity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public string Title { get; set; }

        public int PlaylistPlaytime { get; set; }

        public double Rank { get; set; }

        public ICollection<PlaylistTrack> Tracks { get; set; }

        public ICollection<PlaylistGenre> Genres { get; set; }

        //image!!!

    }
}
