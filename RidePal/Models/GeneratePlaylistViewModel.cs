using Newtonsoft.Json;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Models
{
    public class GeneratePlaylistViewModel
    {
        [Required, MinLength(3), MaxLength(50)]
        [DisplayName("Name:")]
        public string Title { get; set; }

        [Required]
        [DisplayName("From:")]
        public string StartLocationName { get; set; }

        [Required]
        [DisplayName("To:")]
        public string DestinationName { get; set; }
        
        public bool IsSelectedMetal { get; set; }
        public int MetalPercentage { get; set; }
        public bool IsSelectedRock { get; set; }
        public int RockPercentage { get; set; }
        public bool IsSelectedPop { get; set; }
        public int PopPercentage { get; set; }
        public bool IsSelectedJazz { get; set; }
        public int JazzPercentage { get; set; }

        [DisplayName("Allow tracks from the same artist")]
        public bool RepeatArtist { get; set; }

        [DisplayName("Use top tracks")]
        public bool TopTracks { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
