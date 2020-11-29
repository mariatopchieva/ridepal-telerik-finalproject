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
        Task<PlaylistDTO> AdminGetPlaylistByIdAsync(int id);

        Task<IEnumerable<PlaylistDTO>> GetAllPlaylistsAsync();

        Task<bool> EditPlaylistAsync(EditPlaylistDTO editPlaylistDTO);

        Task<PlaylistDTO> AttachImage(PlaylistDTO PlaylistDTO);

        Task<bool> DeletePlaylistAsync(int id);

        Task<bool> UndoDeletePlaylistAsync(int id);

        Task<IEnumerable<PlaylistDTO>> GetPlaylistsOfUserAsync(int userId);

        Task<int> GetHighestPlaytimeAsync();

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

        Task<IEnumerable<PlaylistDTO>> FilterAndReturnPlaylistsPerPageAsync(string name, List<string> genres, List<int> durationLimits, int currentPage);

        Task<int> FilterPlaylistsAndReturnCountAsync(string name, List<string> genres, List<int> durationLimits);

        int GetPageCount();

        int GetPageCountOfCollection(int userId, string collectionName);

        int GetPageCountOfFilteredCollection(int filteredPlaylistsCount);

        IEnumerable<PlaylistDTO> GetFilteredPlaylistsPerPage(int currentPage, IEnumerable<PlaylistDTO> playlists);

        IEnumerable<PlaylistDTO> GetPlaylistsPerPage(int currentPage);

        IEnumerable<PlaylistDTO> GetPlaylistsPerPageOfCollection(int currentPage, int userId, string collectionName);
    }
}
