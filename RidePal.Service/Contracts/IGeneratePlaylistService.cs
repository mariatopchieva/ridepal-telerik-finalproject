using RidePal.Data.Models;
using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IGeneratePlaylistService
    {
        Task<double> GetTravelDuration(string startLocationName, string destinationName);
        Task<IEnumerable<Track>> GetTracksByPreferredGenre(string genre, double travelDuration,
            Dictionary<string, int> genrePercentage);

        Task<PlaylistDTO> GeneratePlaylist(GeneratePlaylistDTO playlistDTO);
    }
}
