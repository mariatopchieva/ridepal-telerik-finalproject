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

            var oldPlaylistGenres = this.context.PlaylistGenres.Where(x => x.PlaylistId == playlist.Id);
            var oldGenresStringList = oldPlaylistGenres.Select(x => x.Genre.Name).ToList();
            var newGenresStringList = editPlaylistDTO.GenrePercentage.Where(x => x.Value > 0).Select(y => y.Key).ToList(); 

            // create new PlaylistGenres за онези, които ги има в новите, но не и в старите 
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

            // ако новите жанрове не съвпадат с новите => update IsDeleted = true
            var genresToDelete = new List<string>();
            foreach (var oldGenre in oldGenresStringList)
            {
                if (!newGenresStringList.Contains(oldGenre))
                {
                    genresToDelete.Add(oldGenre);
                }
            }

            await oldPlaylistGenres.Where(x => genresToDelete.Contains(x.Genre.Name)).ForEachAsync(x => x.IsDeleted = true);

            //test this! GetGenresCount
            var newGenresCount = this.context.PlaylistGenres.Where(x => x.Id == editPlaylistDTO.Id).GroupBy(x => x.GenreId)
                .Select(x => x.First()).ToList().Count();

            playlist.GenresCount = newGenresCount;

            playlist.Title = editPlaylistDTO.Title;

            playlist.ModifiedOn = this.dateTimeProvider.GetDateTime();

            await this.context.SaveChangesAsync();

            return new PlaylistDTO(playlist);
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

        //GetAllGenres да връща списък на жанровете като стринг 

        //GetByUserId => my playlists my favorites

        //get all genres / artists/ albums in a playlist

        public async Task<int> GetHighestPlaytimeAsync()

        {
            var playlist = await this
                .context.Playlists
                .Where(playlist => playlist.IsDeleted == false).OrderByDescending(x => x.PlaylistPlaytime).FirstOrDefaultAsync();

            if (playlist == null)
            {
                return 0;
            }

            return (int)playlist.PlaylistPlaytime;
        }

        //GetShortestPlaytime

        //Sort => by default playlists shoud be sorted by average rank descending

        //Filter by name, genre and duration

        //Get All => show average rank and total playtime

        //Georgi//Get by Id (Details) => lists of artists/ tracks; play a preview

        //Edit/ delete Playlist => (admin)List all/ (user)My playlists

        //Pagination

        //Add authorization restrictions => here?


    }
}
