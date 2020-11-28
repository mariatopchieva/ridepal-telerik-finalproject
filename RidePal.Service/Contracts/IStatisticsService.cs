using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IStatisticsService
    {
        Task<int> TrackCount();
        Task<int> ArtistCount();
        Task<int> GenreCount();
        Task<int> PlaylistCount();
        Task<int> UserCount();
        Task<IList<ArtistDTO>> FeaturedArtists();
        Task<IList<PlaylistDTO>> TopPlaylists();
    }
}
