using RidePal.Data.Context;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using RidePal.Data.Models.BingAPIModels;
using System.Linq;
using RidePal.Service.Contracts;
using RidePal.Data.Models;
using RidePal.Service.DTO;
using Microsoft.EntityFrameworkCore.Internal;

namespace RidePal.Service
{
    public class GeneratePlaylistService : IGeneratePlaylistService
    {
        static string key = "AjFGSA6G9iqex6iC7TYNxLO8Q4w4u2EccyEZnIR8wP2FUU6Z6npKm9AQfcrDDjbJ";
        private readonly RidePalDbContext context;
        HttpClient client = new HttpClient();
        ResourceSet bingResourceSet = new ResourceSet();

        public GeneratePlaylistService(RidePalDbContext _context)
        {
            this.context = _context;
        }

        public async Task<double> GetTravelDuration(string startLocationName, string destinationName)
        {
            string bingMapsRequestUrl = $"https://dev.virtualearth.net/REST/v1/Routes/Driving?wp.0={startLocationName}&wp.1={destinationName}&avoid=minimizeTolls&travelMode=driving&key={key}";


            using (var response = await client.GetAsync(bingMapsRequestUrl))
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                bingResourceSet = JsonSerializer.Deserialize<ResourceSet>(jsonString);
            }

            return bingResourceSet.resourceSets.Select(resource => resource.resources).First()
                .Select(resource => resource.travelDuration).First();
        }


        public async Task<IEnumerable<Track>> GetTracksByPreferredGenre(string genre, double travelDuration, 
            Dictionary<string, int> genrePercentage)
        {

            double durationPerGenre = (genrePercentage[genre] * travelDuration) / 100;

            double currentSongDuration = 0.0;

            //repeats artists
            var songs = context.Tracks.Where(x => x.Genre.Name == genre).AsEnumerable();

            var artists = songs.Select(x => x.ArtistId).Distinct().ToList();

            var songsAllUniqueArtists = songs.Where(x => artists.Contains(x.ArtistId));

            //mytest => does not repeat artist
            var songsUniqueArtistsPerGenre = context.Tracks.Where(x => x.Genre.Name == genre).Where(x => artists.Contains(x.ArtistId));

            Random randomGenerator = new Random();
            List<Track> playlist = new List<Track>();

            for (double i = 0; i < durationPerGenre; i += currentSongDuration)
            {
                int randomNumber = randomGenerator.Next(1, songs.Count());
                var song = songs.ElementAt(randomNumber);
                currentSongDuration = song.TrackDuration;
                playlist.Add(song);
            }

            return playlist;
        }



        public async Task<PlaylistDTO> GeneratePlaylist(GeneratePlaylistDTO playlistDTO)
        {
            // playlistDTO => Playlist model to RidePalDbContext
            double travelDuration = await GetTravelDuration(playlistDTO.StartLocationName, playlistDTO.DestinationName);
            double minPlaytime = travelDuration - 300;
            double maxPlaytime = travelDuration + 300;

            List<Track> tracks = new List<Track>();
            tracks.AddRange(GetTracksByPreferredGenre("rock", travelDuration, playlistDTO.GenrePercentage).Result);
            tracks.AddRange(GetTracksByPreferredGenre("metal", travelDuration, playlistDTO.GenrePercentage).Result);
            tracks.AddRange(GetTracksByPreferredGenre("pop", travelDuration, playlistDTO.GenrePercentage).Result);
            tracks.AddRange(GetTracksByPreferredGenre("jazz", travelDuration, playlistDTO.GenrePercentage).Result);

            


            var playlist = new Playlist()
            {
                Title = playlistDTO.PlaylistName,
                
            };

            






            return new PlaylistDTO();
        }

        //public async Task<BeerDTO> CreateBeerAsync(BeerDTO beerDTO)
        //{
        //    var beer = new Beer()
        //    {
        //        Name = beerDTO.Name,
        //        Description = beerDTO.Description,
        //        BreweryId = beerDTO.BreweryId,
        //        ABV = beerDTO.ABV,
        //        StyleId = beerDTO.StyleId,
        //        CreatedOn = this.dateTimeProvider.GetDateTime()
        //    };

        //    await this._context.Beers.AddAsync(beer);
        //    await this._context.SaveChangesAsync();

        //    return beerDTO;
        //}


    }
}
