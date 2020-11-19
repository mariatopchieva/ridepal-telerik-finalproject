using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IPlaylistService
    {
        Task<PlaylistDTO> GetPlaylistByIdAsync(int id);

        Task<IEnumerable<PlaylistDTO>> GetAllPlaylistsAsync();

        Task<PlaylistDTO> EditPlaylistAsync(EditPlaylistDTO editPlaylistDTO);

        Task<bool> DeletePlaylist(int id);

        Task<IEnumerable<PlaylistDTO>> GetPlaylistOfUser(int userId);

        Task<long> GetHighestPlaytimeAsync();

    }
}
