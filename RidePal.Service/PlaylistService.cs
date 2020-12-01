using Microsoft.EntityFrameworkCore;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;
using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service
{
    public class PlaylistService : IPlaylistService
    {
        const int pageSize = 9;
        private readonly RidePalDbContext context;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IPixaBayImageService imageService;

        public PlaylistService(RidePalDbContext _context, IDateTimeProvider _dateTimeProvider, IPixaBayImageService imageService)
        {
            this.context = _context;
            this.dateTimeProvider = _dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            this.imageService = imageService;
        }

        /// <summary>
        /// Performs search for a playlist by a given ID in the database 
        /// </summary>
        /// <param name="id">The ID of the playlist to search for</param>
        /// <returns>If successful, returns a DTO of the found playlist</returns>
        public async Task<PlaylistDTO> GetPlaylistByIdAsync(int id)
        {
            var playlist = await this.context.Playlists
                                .Where(playlist => playlist.IsDeleted == false)
                                .FirstOrDefaultAsync(playlist => playlist.Id == id);

            if (playlist == null)
            {
                throw new ArgumentNullException("No such playlist was found in the database.");
            }

            var playlistDTO = new PlaylistDTO(playlist);

            return playlistDTO;
        }

        /// <summary>
        /// Performs search for a playlist by a given ID in the database 
        /// </summary>
        /// <param name="id">The ID of the playlist to search for</param>
        /// <returns>If successful, returns a DTO of the found playlist</returns>
        public async Task<PlaylistDTO> AdminGetPlaylistByIdAsync(int id)
        {
            var playlist = await this.context.Playlists
                                            .FirstOrDefaultAsync(playlist => playlist.Id == id);

            if (playlist == null)
            {
                throw new ArgumentNullException("No such playlist was found in the database.");
            }

            var playlistDTO = new PlaylistDTO(playlist);

            return playlistDTO;
        }

        /// <summary>
        /// Collects all playlists in the database and orders them in descending order by rank
        /// </summary>
        /// <returns>Collection of all playlists in the database</returns>
        public async Task<IEnumerable<PlaylistDTO>> GetAllPlaylistsAsync()
        {
            var playlistsDTO = await this.context.Playlists
                                    .Where(playlist => playlist.IsDeleted == false)
                                    .OrderByDescending(playlist => playlist.Rank)
                                    .Select(playlist => new PlaylistDTO(playlist))
                                    .ToListAsync();

            if (playlistsDTO == null)
            {
                throw new ArgumentNullException("No playlists have been found.");
            }

            return playlistsDTO;
        }

        /// <summary>
        /// Edits a playlist's title and/or genres by provided user's input
        /// </summary>
        /// <param name="editPlaylistDTO">DTO of the playlist with the new data</param>
        /// <returns>DTO of the edited playlist</returns>
        public async Task<bool> EditPlaylistAsync(EditPlaylistDTO editPlaylistDTO)
        {
            if (editPlaylistDTO == null)
            {
                throw new ArgumentNullException("No playlist data provided.");
            }

            var playlist = await this.context.Playlists
                .FirstOrDefaultAsync(playlist => playlist.Id == editPlaylistDTO.Id)
                ?? throw new ArgumentNullException("Playlist not found.");

            var genresToAdd = editPlaylistDTO.GenrePercentage.Where(g => g.Value > 0).Select(g => g.Key).ToList();

            var allGenres = await this.context.Genres.ToListAsync();
            var playlistGenreEntries = await this.context.PlaylistGenres
                                                                .Include(plg => plg.Genre)
                                                                .Where(plg => plg.PlaylistId == editPlaylistDTO.Id)
                                                                .ToListAsync();


            foreach (var genre in playlistGenreEntries)
            {
                if (genresToAdd.FirstOrDefault(g => g == genre.Genre.Name) != null)
                {
                    if (genre.IsDeleted == true)
                    {
                        genre.IsDeleted = false;
                    }
                }
                else
                {
                    genre.IsDeleted = true;
                }
            }

            List<PlaylistGenre> newEntries = new List<PlaylistGenre>();
            foreach (var item in genresToAdd)
            {
                if (!playlistGenreEntries.Select(pge => pge.Genre.Name).Contains(item))
                {
                    var genreId = allGenres.FirstOrDefault(g => g.Name == item).Id;
                    newEntries.Add(new PlaylistGenre(genreId, editPlaylistDTO.Id));
                }
            }

            if (editPlaylistDTO.Title != null) { playlist.Title = editPlaylistDTO.Title; }
            if (genresToAdd != null) { playlist.GenresCount = genresToAdd.Count(); }
            playlist.ModifiedOn = this.dateTimeProvider.GetDateTime();
            await this.context.AddRangeAsync(newEntries);

            await this.context.SaveChangesAsync();

            var playlistFromDB = this.context.Playlists
                .Include(p => p.Genres)
                .FirstOrDefaultAsync(playlist => playlist.Id == editPlaylistDTO.Id).Result
                ?? throw new ArgumentNullException("Playlist from database not found.");

            if (editPlaylistDTO.Title != null && playlistFromDB.Title != editPlaylistDTO.Title)
            {
                return false;
            }
            var playlistFromDbGenreNames = await GetPlaylistGenresAsStringAsync(playlist.Id);

            foreach (var gen in genresToAdd)
            {
                if (!playlistFromDbGenreNames.Contains(gen))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Deleted a playlist by setting its IsDeleted property to true
        /// </summary>
        /// <param name="id">ID of the playlist to delete</param>
        /// <returns>True if successful, otherwise false</returns>
        public async Task<bool> DeletePlaylistAsync(int id)
        {
            var playlist = await this.context.Playlists.Where(playlist => playlist.IsDeleted == false)
                                .FirstOrDefaultAsync(playlist => playlist.Id == id);

            if (playlist == null)
            {
                return false;
            }

            playlist.IsDeleted = true;
            playlist.DeletedOn = this.dateTimeProvider.GetDateTime();

            await this.context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Restores the previously deleted playlist by setting its IsDeleted property to false
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if successful, otherwise false</returns>
        public async Task<bool> UndoDeletePlaylistAsync(int id)
        {
            var playlist = await this.context.Playlists.Where(playlist => playlist.Id == id)
                                .FirstOrDefaultAsync();

            if (playlist == null || playlist.IsDeleted == false)
            {
                return false;
            }

            playlist.IsDeleted = false;

            await this.context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Performs search in the database for all playlists created by a specified user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>Collection of DTOs of playlists</returns>
        public async Task<IEnumerable<PlaylistDTO>> GetPlaylistsOfUserAsync(int userId)
        {
            var user = await this.context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentNullException("Invalid user.");
            }

            var playlistsDTO = await this.context.Playlists.Where(x => x.UserId == userId)
                            .Where(x => x.IsDeleted == false)
                            .OrderByDescending(playlist => playlist.Rank)
                            .Select(playlist => new PlaylistDTO(playlist))
                            .ToListAsync();

            if (playlistsDTO == null)
            {
                return null;
            }

            return playlistsDTO;
        }

        /// <summary>
        /// Performs search to find the longest playlist in the database
        /// </summary>
        /// <returns>The duration of the longest playlist in the database</returns>
        public async Task<int> GetHighestPlaytimeAsync()
        {
            var playlist = await this.context.Playlists
                                 .Where(playlist => playlist.IsDeleted == false)
                                 .OrderByDescending(x => x.PlaylistPlaytime).FirstOrDefaultAsync();

            if (playlist == null)
            {
                return 0;
            }

            var minutes = (int)(Math.Ceiling(playlist.PlaylistPlaytime / 60.0));

            return minutes;
        }

        /// <summary>
        /// Performs search in the database to find all music genres
        /// </summary>
        /// <returns>Collection of DTOs of the found genres</returns>
        public async Task<IEnumerable<GenreDTO>> GetAllGenresAsync()
        {
            var genresDTO = await this.context.Genres
                                    .Select(genre => new GenreDTO(genre))
                                    .ToListAsync();

            if (genresDTO.Count == 0)
            {
                throw new ArgumentNullException("No genres have been found.");
            }

            return genresDTO;
        }

        /// <summary>
        /// Upon a given playlist ID, collects its music genres and creates a string of their names
        /// </summary>
        /// <param name="playlistId">ID of the playlist</param>
        /// <returns>String containing all genres in the playlist</returns>
        public async Task<string> GetPlaylistGenresAsStringAsync(int playlistId)
        {
            var genresId = await this.context.PlaylistGenres.Where(x => x.PlaylistId == playlistId)
                                .Where(x => x.IsDeleted == false)
                                .Select(x => x.GenreId).ToListAsync();

            if (genresId.Count == 0)
            {
                throw new ArgumentNullException("No genres have been found.");
            }

            var genreNames = this.context.Genres.Where(x => genresId.Contains(x.Id)).Select(x => x.Name).ToList();

            string genresString = string.Join(", ", genreNames);

            return genresString;
        }

        /// <summary>
        /// Collects and returns all tracks that belong to a certain playlist
        /// </summary>
        /// <param name="playlistId">ID of the playlist</param>
        /// <returns>Collection of all tracks contained in the playlist</returns>
        public async Task<IEnumerable<TrackDTO>> GetPlaylistTracksAsync(int playlistId)
        {
            var playlistDTO = GetPlaylistByIdAsync(playlistId);

            var tracksDTOs = await this.context.PlaylistTracks
                                .Where(x => x.PlaylistId == playlistId)
                                .Select(x => x.Track).Select(track => new TrackDTO(track))
                                .ToListAsync();

            if (tracksDTOs.Count == 0)
            {
                throw new ArgumentNullException("No tracks have been found.");
            }

            foreach (var track in tracksDTOs)
            {
                track.Artist = this.context.Artists.FirstOrDefault(a => a.Id == track.ArtistId);
            }

            return tracksDTOs;
        }

        /// <summary>
        /// Tags a certain playlist as "liked" by a certain user and adds it to his/her Favorites collection
        /// </summary>
        /// <param name="playlistId">ID of the playlist</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>True of successful, otherwise false</returns>
        public async Task<bool> AddPlaylistToFavoritesAsync(int playlistId, int userId)
        {
            var result = await this.context.Favorites
                                            .FirstOrDefaultAsync(fav => fav.PlaylistId == playlistId
                                            && fav.UserId == userId);

            if (result != null)
            {
                if (result.IsFavorite == true)
                {
                    return false;
                }
                else
                {
                    result.IsFavorite = true;
                    await context.SaveChangesAsync();

                    return true;
                }
            }

            PlaylistFavorite item = new PlaylistFavorite
            {
                PlaylistId = playlistId,
                IsFavorite = true,
                UserId = userId
            };

            await context.Favorites.AddAsync(item);
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// If a playlist has been previously tagged as "liked" by a certain user, 
        /// the method removes that tag and removes it from his/her Favorites collection
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> RemovePlaylistFromFavoritesAsync(int playlistId, int userId)
        {
            var result = await this.context.Favorites
                                           .FirstOrDefaultAsync(fav => fav.PlaylistId == playlistId
                                           && fav.UserId == userId);
            if (result == null)
            {
                return false;
            }
            if (result.IsFavorite == false)
            {
                return false;
            }

            result.IsFavorite = false;
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Collects and returns all playlists that a user has liked (added to his Favorites collection)
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>Collection of all playlists liked by the user</returns>
        public async Task<IEnumerable<PlaylistDTO>> GetFavoritePlaylistsOfUser(int userId)
        {
            var playlistFavorites = await this.context.Favorites.Where(pf => pf.UserId == userId)
                                    .Where(pf => pf.IsFavorite == true).ToListAsync();

            if (playlistFavorites == null)
            {
                return null;
            }

            var playlistsDTO = new List<PlaylistDTO>();

            foreach (var item in playlistFavorites)
            {
                var playlist = context.Playlists.First(p => p.Id == item.PlaylistId);
                var playlistDTO = new PlaylistDTO(playlist);
                playlistsDTO.Add(playlistDTO);
            }

            return playlistsDTO;
        }

        /// <summary>
        /// Filters a given collection of playlists and returns only the ones that contain the provided string
        /// </summary>
        /// <param name="name">The string, which must be contained in the filtered playlists' titles</param>
        /// <param name="filteredPlaylists">The initial collection of playlists</param>
        /// <returns>Filtered collection of DTOs of playlists</returns>
        public IEnumerable<PlaylistDTO> FilterPlaylistsByName(string name, IEnumerable<PlaylistDTO> filteredPlaylists)
        {
            var playlists = filteredPlaylists.Where(x => x.Title.Contains(name))
                                              .OrderByDescending(x => x.Rank).ToList();

            return playlists;
        }

        /// <summary>
        /// Filters a given collection of playlists and returns only the ones that contain any of the provided music genres
        /// </summary>
        /// <param name="genres">Collection of the genres</param>
        /// <param name="filteredPlaylists">The initial collection of playlists</param>
        /// <returns>Filtered collection of DTOs of playlists</returns>
        public async Task<IEnumerable<PlaylistDTO>> FilterPlaylistsByGenreAsync(List<string> genres, IEnumerable<PlaylistDTO> filteredPlaylists)
        {
            List<PlaylistDTO> finalList = new List<PlaylistDTO>();

            foreach (var playlist in filteredPlaylists)
            {
                var genresId = await this.context.PlaylistGenres.Where(x => x.PlaylistId == playlist.Id)
                                .Where(x => x.IsDeleted == false)
                                .Select(x => x.GenreId).ToListAsync();

                var genreNames = this.context.Genres.Where(x => genresId.Contains(x.Id)).Select(x => x.Name).ToList();

                if (genreNames.Intersect(genres).Any())
                {
                    finalList.Add(playlist);
                }
            }

            return finalList;
        }

        /// <summary>
        /// Filters a given collection of playlists and returns only the ones with duration within a given range
        /// </summary>
        /// <param name="durationLimits">Upper and lower limit for the filtered playlists' duration</param>
        /// <param name="filteredPlaylists">The initial collection of playlists</param>
        /// <returns>Filtered collection of DTOs of playlists</returns>
        public IEnumerable<PlaylistDTO> FilterPlaylistsByDuration(List<int> durationLimits, IEnumerable<PlaylistDTO> filteredPlaylists)
        {
            var maxDuration = durationLimits[1] * 60;
            var playlists = filteredPlaylists.Where(x => x.PlaylistPlaytime >= durationLimits[0] && x.PlaylistPlaytime <= maxDuration)
                                              .ToList();

            return playlists;
        }

        /// <summary>
        /// A parent method, which receives the filtration parameters of the user and calls the respective methods to filter the collection
        /// </summary>
        /// <param name="name">The string, which must be contained in the filtered playlists' titles</param>
        /// <param name="genres">Collection of the genres</param>
        /// <param name="durationLimits">Upper and lower limit for the filtered playlists' duration</param>
        /// <returns>Filtered collection of DTOs of playlists</returns>
        public async Task<IEnumerable<PlaylistDTO>> FilterPlaylistsMasterAsync(string name, List<string> genres, List<int> durationLimits)
        {
            var filteredPlaylistsDTO = await GetAllPlaylistsAsync();

            if (name != null)
            {
                filteredPlaylistsDTO = FilterPlaylistsByName(name, filteredPlaylistsDTO).ToList();
            }

            if (genres.Count > 0)
            {
                filteredPlaylistsDTO = await FilterPlaylistsByGenreAsync(genres, filteredPlaylistsDTO);
            }

            if (durationLimits.Count > 1) //check default values
            {
                filteredPlaylistsDTO = FilterPlaylistsByDuration(durationLimits, filteredPlaylistsDTO).ToList();
            }

            if (filteredPlaylistsDTO == null) //къде да проверявам за Null => тук или при всеки от 3те метода?
            {
                throw new ArgumentNullException("No playlists meet the filter criteria."); //or no exception???
            }

            return filteredPlaylistsDTO;
        }

        /// <summary>
        /// A parent method, which receives the filtration parameters of the user and calls the respective methods to filter the collection
        /// </summary>
        /// <param name="name">The string, which must be contained in the filtered playlists' titles</param>
        /// <param name="genres">Collection of the genres</param>
        /// <param name="durationLimits">Upper and lower limit for the filtered playlists' duration</param>
        /// <returns>Filtered collection of DTOs of playlists</returns>
        public async Task<int> FilterPlaylistsAndReturnCountAsync(string name, List<string> genres, List<int> durationLimits)
        {
            var filteredPlaylistsDTO = await GetAllPlaylistsAsync();

            if (name != null)
            {
                filteredPlaylistsDTO = FilterPlaylistsByName(name, filteredPlaylistsDTO).ToList();
            }

            if (genres.Count > 0)
            {
                filteredPlaylistsDTO = await FilterPlaylistsByGenreAsync(genres, filteredPlaylistsDTO);
            }

            if (durationLimits.Count > 1) //check default values
            {
                filteredPlaylistsDTO = FilterPlaylistsByDuration(durationLimits, filteredPlaylistsDTO).ToList();
            }

            if (filteredPlaylistsDTO == null) //къде да проверявам за Null => тук или при всеки от 3те метода?
            {
                throw new ArgumentNullException("No playlists meet the filter criteria."); //or no exception???
            }

            return filteredPlaylistsDTO.Count();
        }

        /// <summary>
        /// A parent method, which receives the filtration parameters of the user and calls the respective methods to filter the collection
        /// </summary>
        /// <param name="name">The string, which must be contained in the filtered playlists' titles</param>
        /// <param name="genres">Collection of the genres</param>
        /// <param name="durationLimits">Upper and lower limit for the filtered playlists' duration</param>
        /// <returns>Filtered collection of DTOs of playlists</returns>
        public async Task<IEnumerable<PlaylistDTO>> FilterAndReturnPlaylistsPerPageAsync(string name, List<string> genres, List<int> durationLimits, int currentPage)
        {
            var filteredPlaylistsDTO = await GetAllPlaylistsAsync();

            if (name != null)
            {
                filteredPlaylistsDTO = FilterPlaylistsByName(name, filteredPlaylistsDTO).ToList();
            }

            if (genres.Count > 0)
            {
                filteredPlaylistsDTO = await FilterPlaylistsByGenreAsync(genres, filteredPlaylistsDTO);
            }

            if (durationLimits.Count > 1) //check default values
            {
                filteredPlaylistsDTO = FilterPlaylistsByDuration(durationLimits, filteredPlaylistsDTO).ToList();
            }

            if (filteredPlaylistsDTO == null) //къде да проверявам за Null => тук или при всеки от 3те метода?
            {
                throw new ArgumentNullException("No playlists meet the filter criteria."); //or no exception???
            }

            var playlists = GetFilteredPlaylistsPerPage(currentPage, filteredPlaylistsDTO);

            return playlists;
        }

        /// <summary>
        /// Calculates the total number of pages for the Playlists' Index page
        /// </summary>
        /// <returns>Total number of pages</returns>
        public int GetPageCount()
        {
            var count = this.context.Playlists.Count();

            var totalPages = Math.Ceiling((double)count / pageSize);

            return (int)totalPages;
        }

        /// <summary>
        /// Calculates the total number of pages for the filtered Playlists' Index page
        /// </summary>
        /// <returns>Total number of pages</returns>
        public int GetPageCountOfFilteredCollection(int filteredPlaylistsCount)
        {
            var totalPages = Math.Ceiling((double)filteredPlaylistsCount / pageSize);

            return (int)totalPages;
        }

        /// <summary>
        /// Calculates the total number of pages for the views of My playlists and Favorite playlists
        /// </summary>
        /// <returns>Total number of pages</returns>
        public int GetPageCountOfCollection(int userId, string collectionName)
        {
            List<PlaylistDTO> collection = new List<PlaylistDTO>();

            if(collectionName == "myPlaylists")
            {
                collection = GetPlaylistsOfUserAsync(userId).Result.ToList();
            }
            
            if(collectionName == "favorites")
            {
                collection = GetFavoritePlaylistsOfUser(userId).Result.ToList();
            }

            var count = collection.Count();

            var totalPages = Math.Ceiling((double)count / pageSize);

            return (int)totalPages;
        }

        /// <summary>
        /// Prepares a collection of the exact number of playlists to load on the current page of the Playlists' Index view
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns>Collection of DTOs of playlists to load on the current page</returns>
        public IEnumerable<PlaylistDTO> GetPlaylistsPerPage(int currentPage)
        {
            var playlists = GetAllPlaylistsAsync();

            var resultPlaylistsDTO = currentPage == 1 ? playlists.Result.Take(pageSize) : playlists.Result.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return resultPlaylistsDTO;
        }

        /// <summary>
        /// Prepares a collection of the exact number of playlists to load on the current page of the filtered Playlists' Index view
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns>Collection of DTOs of playlists to load on the current page</returns>
        public IEnumerable<PlaylistDTO> GetFilteredPlaylistsPerPage(int currentPage, IEnumerable<PlaylistDTO> playlists)
        {
            var resultPlaylistsDTO = currentPage == 1 ? playlists.Take(pageSize) : playlists.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return resultPlaylistsDTO;
        }

        /// <summary>
        /// Prepares a collection of the exact number of playlists to load on the current pages of the views of My playlists and Favorite playlists
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns>Collection of DTOs of playlists to load on the current page</returns>
        public IEnumerable<PlaylistDTO> GetPlaylistsPerPageOfCollection(int currentPage, int userId, string collectionName)
        {
            List<PlaylistDTO> collection = new List<PlaylistDTO>();

            if (collectionName == "myPlaylists")
            {
                collection = GetPlaylistsOfUserAsync(userId).Result.ToList();
            }

            if (collectionName == "favorites")
            {
                collection = GetFavoritePlaylistsOfUser(userId).Result.ToList();
            }

            var resultPlaylistsDTO = currentPage == 1 ? collection.Take(pageSize) : collection.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return resultPlaylistsDTO;
        }

        /// <summary>
        /// Attaches an image to a given playlist by assigning its filepath as a property of the playlist
        /// </summary>
        /// <param name="playlistDTO">The playlist to attach an image to</param>
        /// <returns>DTO of the playlist with already attached image</returns>
        public async Task<PlaylistDTO> AttachImage(PlaylistDTO playlistDTO)
        {
            var playlist = await this.context.Playlists.FirstOrDefaultAsync(pl => pl.Title == playlistDTO.Title);

            if (playlist == null)
            {
                throw new ArgumentNullException("Playlist not found.");
            }

            playlist.FilePath = await this.imageService.AssignImage(playlistDTO);

            await this.context.SaveChangesAsync();

            return new PlaylistDTO(playlist);
        }
    }
}
