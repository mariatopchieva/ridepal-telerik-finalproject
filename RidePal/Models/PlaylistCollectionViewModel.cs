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
    public class PlaylistCollectionViewModel
    {

        public IEnumerable<PlaylistViewModel> Playlists { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

    }
}

