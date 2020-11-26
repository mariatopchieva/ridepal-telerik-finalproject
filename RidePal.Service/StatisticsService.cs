using Microsoft.EntityFrameworkCore;
using RidePal.Data.Context;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Service
{
    public class StatisticsService : IStatisticsService
    {
        private RidePalDbContext context;
        private readonly IPlaylistService playlistService;

        public StatisticsService(RidePalDbContext context, IPlaylistService plService)
        {
            this.context = context;
            this.playlistService = plService;
        }

        public async Task<int> TrackCount()
        {
            int trackCount = await this.context.Tracks.CountAsync(track => track.IsDeleted == false);
            return trackCount;
        }

        public async Task<int> ArtistCount()
        {
            int artistCount = await this.context.Artists.CountAsync();
            return artistCount;
        }

        public async Task<int> GenreCount()
        {
            int genreCount = await this.context.Genres.CountAsync();
            return genreCount;
        }

        public async Task<int> PlaylistCount()
        {
            int playlistCount = await this.context.Playlists.CountAsync(pl => pl.IsDeleted == false);
            return playlistCount;
        }

        public async Task<int> UserCount()
        {
            int userCount = await this.context.Users.CountAsync(user => user.IsDeleted == false);
            return userCount;
        }

        public async Task<IList<PlaylistDTO>> TopPlaylists()
        {
            var topPlaylistDTOs = await this.context.Playlists
                                            .Where(pl => pl.IsDeleted == false)
                                            .OrderBy(pl => pl.Rank)
                                            .Take(3)
                                            .Select(pl => new PlaylistDTO(pl))
                                            .ToListAsync();

            foreach (var pl in topPlaylistDTOs)
            {
                pl.GenreString = await this.playlistService.GetPlaylistGenresAsStringAsync(pl.Id);
            }

            return topPlaylistDTOs;
        }

        public async Task<IList<ArtistDTO>> FeaturedArtists()
        {
            var topArtistsDTOs = await this.context.Artists
                                                    .Where(artist => artist.ArtistName != null && artist.ArtistPictureURL != null)
                                                    .ToListAsync();

            ListShuffler.Shuffle(topArtistsDTOs);
            var featuredArtists = topArtistsDTOs.Take(3)
                                                .Select(pl => new ArtistDTO(pl))
                                                .ToList();

            return featuredArtists;
        }
    }

    public static class ListShuffler
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
