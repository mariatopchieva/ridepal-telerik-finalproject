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
    public class FilteredPlaylistsViewModel
    {
        public FilteredPlaylistsViewModel()
        {
            this.Playlists = new List<PlaylistViewModel>();

            this.FilterCriteria = null;
        }

        public IEnumerable<PlaylistViewModel> Playlists { get; set; }

        public IEnumerable<GenreDTO> AllGenres { get; set; }

        public FilterCriteria FilterCriteria { get; set; }
    }
}

