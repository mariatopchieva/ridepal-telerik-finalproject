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


        public async Task<IEnumerable<Track>> GetTracksByPreferredGenre(string genre, double travelDuration, //ADD AWAIT
            Dictionary<string, int> genrePercentage, bool repeatArtist, bool useTopTracks)
        {
            List<Track> tracksPerGenre = context.Tracks.Where(x => x.Genre.Name == genre).ToList();
            List<Track> tracks = new List<Track>();
            bool useRandomGenerator;

            if(repeatArtist)
            {
                if(useTopTracks)
                {
                    tracks = tracksPerGenre.OrderByDescending(x => x.TrackRank).ToList();
                    useRandomGenerator = false;
                }
                else
                {
                    tracks = tracksPerGenre;
                    useRandomGenerator = true;
                }
            }
            else
            {
                var tracksUniqueArtists = tracksPerGenre.GroupBy(y => y.ArtistId).Select(z => z.First()).ToList();

                if (useTopTracks)
                {
                    tracks = tracksUniqueArtists.OrderByDescending(x => x.TrackRank).ToList();
                    useRandomGenerator = false;
                }
                else
                {
                    tracks = tracksUniqueArtists;
                    useRandomGenerator = true;
                }
            }

            double durationPerGenre = (genrePercentage[genre] * travelDuration) / 100;

            double currentTrackDuration = 0.0;

            Random randomGenerator = new Random();
            List<Track> playlist = new List<Track>();
            int counter = 1;
            Track currentTrack;

            for (double i = 0; i < durationPerGenre; i += currentTrackDuration)
            {
                if(useRandomGenerator)
                {
                    int randomNumber = randomGenerator.Next(1, tracks.Count());
                    currentTrack = tracks.ElementAt(randomNumber);
                }
                else
                {
                    currentTrack = tracks.ElementAt(counter);
                    counter++;
                }

                if(!playlist.Contains(currentTrack))
                {
                    currentTrackDuration = currentTrack.TrackDuration;
                    playlist.Add(currentTrack);
                }
            }

            return playlist;
        }


        public async Task<PlaylistDTO> GeneratePlaylist(GeneratePlaylistDTO playlistDTO)
        {
            double travelDuration = await GetTravelDuration(playlistDTO.StartLocationName, playlistDTO.DestinationName);
            double minPlaytime = travelDuration - 300;
            double maxPlaytime = travelDuration + 300;

            List<Track> tracks = new List<Track>();
            tracks.AddRange(GetTracksByPreferredGenre("rock", travelDuration, playlistDTO.GenrePercentage, 
                playlistDTO.RepeatArtist, playlistDTO.UseTopTracks).Result);
            tracks.AddRange(GetTracksByPreferredGenre("metal", travelDuration, playlistDTO.GenrePercentage, 
                playlistDTO.RepeatArtist, playlistDTO.UseTopTracks).Result);
            tracks.AddRange(GetTracksByPreferredGenre("pop", travelDuration, playlistDTO.GenrePercentage, 
                playlistDTO.RepeatArtist, playlistDTO.UseTopTracks).Result);
            tracks.AddRange(GetTracksByPreferredGenre("jazz", travelDuration, playlistDTO.GenrePercentage, 
                playlistDTO.RepeatArtist, playlistDTO.UseTopTracks).Result);

            //check playlist duration => if longer than maxPlaytime, remove track с duration между 0 и 5 мин; 
            //                           if shorter than minPlaytime, add track с duration между 0 и 5 мин; 
            //                           set PlaylistPlaytime & other properties

            //mix the songs
            
            //save playtist to user's profile


            // playlistDTO => Playlist model to RidePalDbContext
            var playlist = new Playlist()
            {
                Title = playlistDTO.PlaylistName,
                
            };



            await this.context.Playlists.AddAsync(playlist);
            await this.context.SaveChangesAsync();

            //map playlist to PlaylistDTO
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
