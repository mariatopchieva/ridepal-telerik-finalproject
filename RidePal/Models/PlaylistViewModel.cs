using RidePal.Data.Models;
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
        [Key]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        [DisplayName("Name:")]
        public string Title { get; set; }

        public int PlaylistPlaytime { get; set; }
        public double Rank { get; set; }
        public ICollection<PlaylistTrack> Tracks { get; set; }

        public ICollection<PlaylistGenre> Genres { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [DisplayName("Tracks from the same artist allowed")]
        public bool IsAllowedSameArtistTracks { get; set; }

        [DisplayName("Top tracks used")]
        public bool ArePreferredTopTracks { get; set; }
    }
}
