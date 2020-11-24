using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service.Contracts
{
    public interface IPlaylistService
    {
        Task<PlaylistDTO> GetPlaylistByIdAsync(long id);

        Task<IEnumerable<PlaylistDTO>> GetAllPlaylistsAsync();

        Task<PlaylistDTO> EditPlaylistAsync(EditPlaylistDTO editPlaylistDTO);

        Task<bool> DeletePlaylistAsync(long id);

        Task<bool> ReverseDeletePlaylistAsync(long id);

        Task<IEnumerable<PlaylistDTO>> GetPlaylistsOfUserAsync(int userId);

        Task<long> GetHighestPlaytimeAsync();

        Task<IEnumerable<GenreDTO>> GetAllGenresAsync();

        Task<string> GetPlaylistGenresAsStringAsync(int playlistId);

        Task<IEnumerable<TrackDTO>> GetPlaylistTracksAsync(int playlistId);

        Task<bool> AddPlaylistToFavoritesAsync(int playlistId, int userId);

        Task<bool> RemovePlaylistFromFavoritesAsync(int playlistId, int userId);

        Task<IEnumerable<PlaylistDTO>> GetFavoritePlaylistsOfUser(int userId);

        IEnumerable<PlaylistDTO> FilterPlaylistsByName(string name, IEnumerable<PlaylistDTO> filteredPlaylists);

        Task<IEnumerable<PlaylistDTO>> FilterPlaylistsByGenreAsync(List<string> genres, IEnumerable<PlaylistDTO> filteredPlaylists);

        IEnumerable<PlaylistDTO> FilterPlaylistsByDuration(List<int> durationLimits, IEnumerable<PlaylistDTO> filteredPlaylists);

        Task<IEnumerable<PlaylistDTO>> FilterPlaylistsMasterAsync(string name, List<string> genres, List<int> durationLimits);

        Task<IEnumerable<PlaylistDTO>> SortPlaylistsByDurationAsync();
    }
}
