﻿using RidePal.Service.DTO;
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

        Task<IEnumerable<PlaylistDTO>> FilterPlaylistsByNameAsync(string name);

        Task<IEnumerable<PlaylistDTO>> FilterPlaylistsByGenreAsync(List<string> genres);

        Task<IEnumerable<PlaylistDTO>> FilterPlaylistsByDurationAsync(List<int> durationLimits);

        Task<IEnumerable<PlaylistDTO>> FilterPlaylistsMasterAsync();

        Task<IEnumerable<PlaylistDTO>> SortPlaylistsByDurationAsync();
    }
}
