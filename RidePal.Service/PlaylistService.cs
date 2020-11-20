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

        public async Task<bool> DeletePlaylist(int id)
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

        public async Task<IEnumerable<PlaylistDTO>> GetPlaylistOfUser(int userId)
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

        //GetByUserId => my favorites; Add Playlist to Favorites; Delete Playlist from Favorites

        //GetAllGenres / GetAllGenresString/ artists/ artist count/ albums in a playlist //Title, Destination from/to

        //Sort => by default playlists shoud be sorted by average rank descending //duration
        //Могат да се сортират и по Playtime

        //Filter by name, genre and duration

        //Pagination

        //Add authorization restrictions => here? Edit/ delete Playlist => (admin)List all/ (user)My playlists

        //Add all methods to IPlaylistService

    }
}
