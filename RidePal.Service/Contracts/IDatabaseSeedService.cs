using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IDatabaseSeedService
    {
        Task DownloadTrackData(string incomingGenre);
        Task TraverseTracklist(string currentTracklistUrl, string incomingGenre);
        IEnumerable<GeneratePlaylistDTO> GeneratePlaylists();
    }
}
