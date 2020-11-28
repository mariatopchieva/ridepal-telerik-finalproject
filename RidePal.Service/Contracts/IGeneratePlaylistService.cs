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
        Task<double> GetTravelDuration(string startLocation, string destination);
        Task<IEnumerable<Track>> GetTracks(string genre, double travelDuration, Dictionary<string, int> genrePercentage, bool repeatArtist);

        Task<IEnumerable<Track>> GetTopTracks(string genre, double travelDuration, Dictionary<string, int> genrePercentage, bool repeatArtist);

        double CalculatePlaytime(List<Track> playlist);

        int CalculateRank(List<Track> playlist);

        string GetPlaytimeString(int playtime);

        public IEnumerable<Track> FinetunePlaytime(double travelDuration, List<Track> playlist, Dictionary<string, int> genrePercentage);

        Task<PlaylistDTO> GeneratePlaylist(GeneratePlaylistDTO playlistDTO);
    }
}
