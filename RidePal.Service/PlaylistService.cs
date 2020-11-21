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
        private readonly RidePalDbContext context;
        private readonly IDateTimeProvider dateTimeProvider;

        public PlaylistService(RidePalDbContext _context, IDateTimeProvider _dateTimeProvider)
        {
            this.context = _context;
            this.dateTimeProvider = _dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<PlaylistDTO> GetPlaylistByIdAsync(long id)
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

            // update IsDeleted = false if the new genres do not include the old genres but they were created and deleted
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

            //update PlaylistGenre list-a na each genre (see Generate service)
        }

        public async Task<bool> DeletePlaylistAsync(long id)
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

        public async Task<IEnumerable<PlaylistDTO>> GetPlaylistsOfUserAsync(int userId)
        {
            var playlistsDTO = await this.context.Playlists.Where(x => x.UserId == userId)
                            .OrderByDescending(playlist => playlist.Rank)
                            .Select(playlist => new PlaylistDTO(playlist))
                            .ToListAsync();

            if (playlistsDTO == null)
            {
                return null;
            }
            
            return playlistsDTO;
        }

        public async Task<long> GetHighestPlaytimeAsync()
        {
            var playlist = await this
                .context.Playlists
                .Where(playlist => playlist.IsDeleted == false).OrderByDescending(x => x.PlaylistPlaytime).FirstOrDefaultAsync();

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
                               .GroupBy(x => x.GenreId).Select(x => x.First())
                               .Select(x => x.GenreId).ToListAsync();

            if (genresId == null)
            {
                throw new ArgumentNullException("No genres have been found.");
            }

            var genreNames = this.context.Genres.Where(x => genresId.Contains(x.Id)).Select(x => x.Name).ToList();

            string genresString = string.Join(", ", genreNames);

            return genresString;
        }

        //Test the three cases of this method
        public async Task<bool> AddPlaylistToFavoritesAsync(int playlistId, int userId)
        {
            var playlist = await context.Playlists.FirstOrDefaultAsync(p => p.Id == playlistId);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (playlist == null || user == null)
            {
                return false;
            }

            PlaylistFavorite item = new PlaylistFavorite
            {
                Playlist = playlist,
                PlaylistId = playlist.Id,
                IsFavorite = true,
                UserId = user.Id,
                User = user
            };

            var favoritesTrue = await this.context.Favorites.Where(pf => pf.UserId == userId)
                                    .Where(pf => pf.IsFavorite == true).ToListAsync();

            var favoritesFalse = await this.context.Favorites.Where(pf => pf.UserId == userId)
                                    .Where(pf => pf.IsFavorite == false).ToListAsync();

            if (favoritesTrue.Any(f => f.Id == item.Id))
            {
                return false; //the playlist has already been liked
            }
            else if (favoritesFalse.Any(f => f.Id == item.Id))
            {
                var playlistToSetFavorite = await this.context.Favorites.FirstOrDefaultAsync(pf => pf.Id == item.Id);
                playlistToSetFavorite.IsFavorite = true;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                await context.Favorites.AddAsync(item);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> RemovePlaylistFromFavoritesAsync(int playlistId, int userId)
        {

            var playlist = await context.Playlists.FirstOrDefaultAsync(p => p.Id == playlistId);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (playlist == null || user == null)
            {
                return false;
            }

            var favoritesSetAsTrue = await GetFavoritePlaylistsOfUser(user.Id);

            if (favoritesSetAsTrue.Any(f => f.Id == playlistId))
            {
                var playlistToRemove = await this.context.Favorites.FirstOrDefaultAsync(pf => pf.PlaylistId == playlistId);
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





        //Могат да се сортират и по Playtime

        //Filter by name, genre and duration

        //Pagination

        //Add authorization restrictions => here? Edit/ delete Playlist => (admin)List all/ (user)My playlists

        //Add all methods to IPlaylistService

    }
}
