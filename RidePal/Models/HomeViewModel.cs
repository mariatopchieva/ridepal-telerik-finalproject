using RidePal.Service.DTO;
using System;
using System.Collections.Generic;

namespace RidePal.Models
{
    public class HomeViewModel
    {
        private readonly int trackCount;
        private readonly int artistCount;
        private readonly int genreCount;
        private readonly int playlistCount;
        private readonly int userCount;
        private readonly IList<PlaylistDTO> topPlaylists;
        private readonly IList<ArtistDTO> featuredArtists;

        public int TrackCount { get; set; }
        public int ArtistCount { get; set; }
        public int GenreCount { get; set; }
        public int PlaylistCount { get; set; }
        public int UserCount { get; set; }
        public IList<PlaylistViewModel> TopPlaylists { get; set; }
        public IList<ArtistDTO>  FeaturedArtists{ get; set; }
    }
}
