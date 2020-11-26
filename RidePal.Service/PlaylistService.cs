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

        public async Task<PlaylistDTO> EditPlaylistAsync(EditPlaylistDTO editPlaylistDTO)
        {
            if(editPlaylistDTO == null)
            {
                throw new ArgumentNullException("No playlist data provided.");
            }

            var playlist = await this.context.Playlists.Where(playlist => playlist.IsDeleted == false)
                                .FirstOrDefaultAsync(playlist => playlist.Id == editPlaylistDTO.Id);

            if (playlist == null)
            {
                throw new ArgumentNullException("Playlist not found in the database.");
            }

            var oldPlaylistGenres = this.context.PlaylistGenres.Where(x => x.PlaylistId == playlist.Id).Where(x => x.IsDeleted == false);
            var oldGenresStringList = oldPlaylistGenres.Select(x => x.Genre.Name).ToList();

            var oldDeletedGenres = this.context.PlaylistGenres.Where(x => x.PlaylistId == playlist.Id).Where(x => x.IsDeleted == true);
            var oldDeletedGenresStringList = oldDeletedGenres.Select(x => x.Genre.Name).ToList();

            var newGenresStringList = editPlaylistDTO.GenrePercentage.Where(x => x.Value > 0).Select(y => y.Key).ToList();
            int newGenresCount = newGenresStringList.Count();

            // create new PlaylistGenres for the new genres that have not been created yet 
            var genresToCreate = new List<string>();
            foreach (var newGenre in newGenresStringList)
            {
                if(!oldGenresStringList.Contains(newGenre))
                {
                    genresToCreate.Add(newGenre);
                }
            }
            List<Genre> newGenres = this.context.Genres.Where(x => genresToCreate.Contains(x.Name)).ToList();
            List<PlaylistGenre> playlistGenres = newGenres.Select(x => new PlaylistGenre(x.Id, editPlaylistDTO.Id)).ToList();

            // update IsDeleted = true if the new genres do not include the old genres
            var genresToDelete = new List<string>();
            foreach (var oldGenre in oldGenresStringList)
            {
                if (!newGenresStringList.Contains(oldGenre))
                {
                    genresToDelete.Add(oldGenre);
                }
            }
            await this.context.PlaylistGenres.Where(x => x.PlaylistId == playlist.Id)
                                             .Where(x => genresToDelete.Contains(x.Genre.Name))
                                             .ForEachAsync(x => x.IsDeleted = true);

            // update IsDeleted = false if the new genres do not include the old genres but they were created and deleted before
            var genresToUndelete = new List<string>();
            foreach (var oldDeletedGenre in oldDeletedGenresStringList)
            {
                if (newGenresStringList.Contains(oldDeletedGenre))
                {
                    genresToUndelete.Add(oldDeletedGenre);
                }
            }
            await this.context.PlaylistGenres.Where(x => x.PlaylistId == playlist.Id)
                                             .Where(x => genresToUndelete.Contains(x.Genre.Name))
                                             .ForEachAsync(x => x.IsDeleted = false);

            playlist.Title = editPlaylistDTO.Title;
            playlist.GenresCount = newGenresCount;
            playlist.ModifiedOn = this.dateTimeProvider.GetDateTime();

            await this.context.PlaylistGenres.AddRangeAsync(playlistGenres);
            await this.context.SaveChangesAsync();

            return new PlaylistDTO(playlist);
        }

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

        public async Task<bool> ReverseDeletePlaylistAsync(int id)
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
                return null; //OR throw new ArgumentNullException("This user has not created any playlists yet.");
            }
            
            return playlistsDTO;
        }

        public async Task<long> GetHighestPlaytimeAsync()
        {
            var playlist = await this.context.Playlists
                                 .Where(playlist => playlist.IsDeleted == false)
                                 .OrderByDescending(x => x.PlaylistPlaytime).FirstOrDefaultAsync();

            if (playlist == null)
            {
                return 0;
            }

            return (long)playlist.PlaylistPlaytime;
        }

        public async Task<IEnumerable<GenreDTO>> GetAllGenresAsync()
        {
            var genresDTO = await this.context.Genres
                                    .Select(genre => new GenreDTO(genre))
                                    .ToListAsync();

            if (genresDTO == null)
            {
                throw new ArgumentNullException("No genres have been found.");
            }

            return genresDTO;
        }

        public async Task<string> GetPlaylistGenresAsStringAsync(int playlistId)
        {
            var genresId = await this.context.PlaylistGenres.Where(x => x.PlaylistId == playlistId)
                                .Where(x => x.IsDeleted == false)
                                .Select(x => x.GenreId).ToListAsync();

            if (genresId == null)
            {
                throw new ArgumentNullException("No genres have been found.");
            }

            var genreNames = this.context.Genres.Where(x => genresId.Contains(x.Id)).Select(x => x.Name).ToList();

            string genresString = string.Join(", ", genreNames);

            return genresString;
        }

        public async Task<IEnumerable<TrackDTO>> GetPlaylistTracksAsync(int playlistId)
        {
            var playlistDTO = GetPlaylistByIdAsync(playlistId);

            var tracksDTOs = await this.context.PlaylistTracks
                                .Where(x => x.PlaylistId == playlistId)
                                .Select(x => x.Track).Select(track => new TrackDTO(track))
                                .ToListAsync();

            if (tracksDTOs == null)
            {
                throw new ArgumentNullException("No tracks have been found.");
            }

            foreach (var track in tracksDTOs)
            {
                track.Artist = this.context.Artists.FirstOrDefault(a => a.Id == track.ArtistId);
            }

            return tracksDTOs;
        }

        //Test the three cases of this method
        public async Task<bool> AddPlaylistToFavoritesAsync(int playlistId, int userId)
        {
            var playlist = await this.context.Playlists
                                .Where(playlist => playlist.IsDeleted == false)
                                .FirstOrDefaultAsync(playlist => playlist.Id == playlistId);

            var user = await this.context.Users
                                .Where(u => u.IsDeleted == false)
                                .FirstOrDefaultAsync(u => u.Id == userId);

            if (playlist == null || user == null)
            {
                return false;
            }

            var playlistsLiked = await this.context.Favorites.Where(pf => pf.UserId == userId)
                                    .Where(pf => pf.IsFavorite == true).ToListAsync();

            var playlistsLikedAndDisliked = await this.context.Favorites.Where(pf => pf.UserId == userId) //PlaylistFavorite item created with False property
                                    .Where(pf => pf.IsFavorite == false).ToListAsync();

            if (playlistsLiked.Any(pf => pf.PlaylistId == playlistId))
            {
                return false; //the playlist has already been liked
            }
            else if (playlistsLikedAndDisliked.Any(pf => pf.PlaylistId == playlistId))
            {
                var playlistToSetAsFavorite = await this.context.Favorites.Where(pf => pf.UserId == userId)
                                                    .Where(pf => pf.PlaylistId == playlistId).FirstOrDefaultAsync();

                playlistToSetAsFavorite.IsFavorite = true;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                PlaylistFavorite item = new PlaylistFavorite
                {
                    Playlist = playlist,
                    PlaylistId = playlist.Id,
                    IsFavorite = true,
                    UserId = user.Id,
                    User = user
                };

                await context.Favorites.AddAsync(item);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> RemovePlaylistFromFavoritesAsync(int playlistId, int userId)
        {
            var playlist = await GetPlaylistByIdAsync(playlistId);

            var user = await this.context.Users
                                .Where(u => u.IsDeleted == false)
                                .FirstOrDefaultAsync(u => u.Id == userId);

            if (playlist == null || user == null)
            {
                return false;
            }

            var playlistsLiked = await GetFavoritePlaylistsOfUser(user.Id);

            if (playlistsLiked.Any(f => f.Id == playlistId))
            {
                var playlistToRemove = await this.context.Favorites.Where(pf => pf.UserId == userId)
                                                    .Where(pf => pf.PlaylistId == playlistId).FirstOrDefaultAsync();

                playlistToRemove.IsFavorite = false;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false; 
            }
        }

        public async Task<IEnumerable<PlaylistDTO>> GetFavoritePlaylistsOfUser(int userId)
        {
            var playlistFavorites = await this.context.Favorites.Where(pf => pf.UserId == userId)
                                    .Where(pf => pf.IsFavorite == true).ToListAsync();

            if (playlistFavorites == null)
            {
                return null; //or exception?
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

        public async Task<IEnumerable<PlaylistDTO>> SortPlaylistsByDurationAsync()
        {
            var playlistsDTO = await this.context.Playlists
                                    .Where(playlist => playlist.IsDeleted == false)
                                    .OrderByDescending(playlist => playlist.PlaylistPlaytime)
                                    .Select(playlist => new PlaylistDTO(playlist))
                                    .ToListAsync();

            if (playlistsDTO == null)
            {
                throw new ArgumentNullException("No playlists have been found.");
            }

            return playlistsDTO;
        }

        public IEnumerable<PlaylistDTO> FilterPlaylistsByName(string name, IEnumerable<PlaylistDTO> filteredPlaylists)
        {
            var playlists = filteredPlaylists.Where(x => x.Title.Contains(name))
                                              .OrderByDescending(x => x.Rank).ToList();
            
            return playlists;
        }

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

        public IEnumerable<PlaylistDTO> FilterPlaylistsByDuration(List<int> durationLimits, IEnumerable<PlaylistDTO> filteredPlaylists)
        {
            var playlists = filteredPlaylists.Where(x => x.PlaylistPlaytime >= durationLimits[0] && x.PlaylistPlaytime <= durationLimits[1])
                                              .ToList();
            
            return playlists;
        }

        public async Task<IEnumerable<PlaylistDTO>> FilterPlaylistsMasterAsync(string name, List<string> genres, List<int> durationLimits)
        {
            var filteredPlaylistsDTO = await GetAllPlaylistsAsync();

            if(name != null)
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

        public int GetPageCount()
        {
            var count = this.context.Playlists.Count();

            var totalPages = Math.Ceiling((double)count / pageSize);

            return (int)totalPages;
        }

        public IEnumerable<PlaylistDTO> GetPlaylistsPerPage(int currentPage)
        {
            var playlists = GetAllPlaylistsAsync();

            var resultPlaylistsDTO = currentPage == 1 ? playlists.Result.Take(pageSize) : playlists.Result.Skip((currentPage - 1) * pageSize).Take(pageSize);

            return resultPlaylistsDTO;
        }

        public async Task<PlaylistDTO> AttachImage(PlaylistDTO playlistDTO)
        {
            var playlist = await this.context.Playlists.FirstOrDefaultAsync(pl => pl.Title == playlistDTO.Title);

            if (playlist == null)
            {
                throw new ArgumentNullException("Playlist not found.");
            }

            playlist.FilePath = await this.imageService.AssignImage(playlistDTO);

            await this.context.SaveChangesAsync();

            return playlistDTO;
        }

        //Pagination

        //Add authorization restrictions => here? Edit/ delete Playlist => (admin)List all/ (user)My playlists
        //                                 => here? Add/ remove Playlist as Favorite => (user)Favorite playlists
    }
}
