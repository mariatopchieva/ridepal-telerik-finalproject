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


        public async Task<PlaylistDTO> GeneratePlaylist(GeneratePlaylistDTO playlistDTO)
        {
            // playlistDTO => Playlist model to RidePalDbContext
            double travelDuration = await GetTravelDuration(playlistDTO.startLocationName, playlistDTO.destinationName);
            double minPlaytime = travelDuration - 300;
            double maxPlaytime = travelDuration + 300;

            double rockDuration = (playlistDTO.genrePercentage["rock"] * travelDuration) / 100;
            double metalDuration = (playlistDTO.genrePercentage["metal"] * travelDuration) / 100;
            double popDuration = (playlistDTO.genrePercentage["pop"] * travelDuration) / 100;
            double jazzDuration = (playlistDTO.genrePercentage["jazz"] * travelDuration) / 100;

            double currentSongDuration = 0.0;
            for (double i = 0; i < rockDuration; i += currentSongDuration)
            {
                currentSongDuration = context.Tracks.Where(x => x.Genre.Name == "rock").FirstOrDefault().TrackDuration;

            }

            //



            var playlist = new Playlist()
            {
                Title = playlistDTO.playlistName,
                
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
