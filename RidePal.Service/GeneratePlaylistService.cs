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
using Microsoft.EntityFrameworkCore;
using RidePal.Service.Providers.Contracts;

namespace RidePal.Service
{
    public class GeneratePlaylistService : IGeneratePlaylistService
    {
        static string key = "AjFGSA6G9iqex6iC7TYNxLO8Q4w4u2EccyEZnIR8wP2FUU6Z6npKm9AQfcrDDjbJ";
        private readonly RidePalDbContext context;
        private readonly IDateTimeProvider dateTimeProvider;
        HttpClient client = new HttpClient();
        ResourceSet bingResourceSet = new ResourceSet();

        public GeneratePlaylistService(RidePalDbContext _context, IDateTimeProvider _dateTimeProvider)
        {
            this.context = _context;
            this.dateTimeProvider = _dateTimeProvider;
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

        
        public async Task<IEnumerable<Track>> GetTopTracks(string genre, double travelDuration,
            Dictionary<string, int> genrePercentage, bool repeatArtist)
        {
            List<Track> tracksPerGenre = await context.Tracks.Where(x => x.Genre.Name == genre).ToListAsync();
            List<Track> allTracksOrdered = new List<Track>();

            if (repeatArtist)
            {
                allTracksOrdered = tracksPerGenre.OrderByDescending(x => x.TrackRank).ToList();
            }
            else
            {
                var tracksUniqueArtists = tracksPerGenre.GroupBy(y => y.ArtistId).Select(z => z.First()).ToList();
                allTracksOrdered = tracksUniqueArtists.OrderByDescending(x => x.TrackRank).ToList();
            }

            List<Track> topTracks = new List<Track>();
            topTracks.AddRange(allTracksOrdered.Take(120));
            allTracksOrdered.RemoveRange(0, 120);

            List<Track> playlist = new List<Track>();

            double durationPerGenre = (genrePercentage[genre] * travelDuration) / 100;

            double currentTrackDuration = 0.0;

            Random randomGenerator = new Random();
            Track currentTrack;

            for (double i = 0, j = 1.0; i < durationPerGenre; i += currentTrackDuration, j++)
            {
                if (j % 5.0 == 0) //every fifth tracks comes from the non-top tracks collection
                {
                    int randomNumber = randomGenerator.Next(1, allTracksOrdered.Count());
                    currentTrack = allTracksOrdered.ElementAt(randomNumber);
                    currentTrackDuration = currentTrack.TrackDuration;
                    playlist.Add(currentTrack);
                    allTracksOrdered.RemoveAt(randomNumber);
                }
                else //4 tracks come from top tracks collection
                {
                    int randomNumber = randomGenerator.Next(1, topTracks.Count());
                    currentTrack = topTracks.ElementAt(randomNumber);
                    currentTrackDuration = currentTrack.TrackDuration;
                    playlist.Add(currentTrack);
                    topTracks.RemoveAt(randomNumber);
                }
            }

            return playlist;
        }


        public async Task<IEnumerable<Track>> GetTracks(string genre, double travelDuration, //ADD AWAIT
            Dictionary<string, int> genrePercentage, bool repeatArtist)
        {
            List<Track> tracksPerGenre = await context.Tracks.Where(x => x.Genre.Name == genre).ToListAsync();
            List<Track> tracks = new List<Track>();

            if(repeatArtist)
            {
                tracks = tracksPerGenre;
            }
            else
            {
                var tracksUniqueArtists = tracksPerGenre.GroupBy(y => y.ArtistId).Select(z => z.First()).ToList();
                tracks = tracksUniqueArtists;
            }

            double durationPerGenre = (genrePercentage[genre] * travelDuration) / 100;

            double currentTrackDuration = 0.0;

            Random randomGenerator = new Random();
            List<Track> playlist = new List<Track>();
            Track currentTrack;

            for (double i = 0; i < durationPerGenre; i += currentTrackDuration)
            {
                int randomNumber = randomGenerator.Next(1, tracks.Count());
                currentTrack = tracks.ElementAt(randomNumber);
                currentTrackDuration = currentTrack.TrackDuration;
                playlist.Add(currentTrack);
                tracks.RemoveAt(randomNumber);
            }

            return playlist;
        }

        public double CalculatePlaytime(List<Track> playlist)
        {
            double playtime = (double)playlist.Select(track => track.TrackDuration).Sum();

            return playtime;
        }

        public int CalculateRank(List<Track> playlist)
        {
            int rank = (int)playlist.Select(track => track.TrackRank).Average();
            
            return rank;
        }


        public IEnumerable<Track> FinetunePlaytime(double travelDuration, List<Track> playlist, 
            Dictionary<string, int> genrePercentage)
        {
            double playtime = CalculatePlaytime(playlist);
            double minPlaytime = travelDuration - 300;
            double maxPlaytime = travelDuration + 300;
            
            if(playtime > maxPlaytime)
            {
                Random randomGenerator = new Random();
                double reducedPlaytime = playtime;

                for (int i = 1; i < playlist.Count; i++)
                {
                    int randomNumber = randomGenerator.Next(1, playlist.Count() - 1);
                    var currentTrack = playlist.ElementAt(randomNumber);

                    reducedPlaytime -= currentTrack.TrackDuration;
                    playlist.Remove(currentTrack);

                    if (reducedPlaytime <= maxPlaytime)
                    {
                        break;
                    }
                }
            }

            return playlist;
        }

        public string GetPlaytimeString(int playlistPlaytime)
        {
            StringBuilder sb = new StringBuilder();

            int hours = playlistPlaytime / 3600;

            int remainingMinutes = (playlistPlaytime % 3600) / 60;

            int remainingSeconds = (playlistPlaytime % 3600) % 60;

            if (hours > 0)
            {
                sb.Append($"{hours}h ");
            }

            if (remainingMinutes >= 0)
            {
                sb.Append($"{remainingMinutes}m ");
            }

            if(remainingSeconds >= 0)
            {
                sb.Append($"{remainingSeconds}s");
            }

            return sb.ToString();
        }

        public async Task<PlaylistDTO> GeneratePlaylist(GeneratePlaylistDTO playlistDTO)
        {
            double travelDuration = await GetTravelDuration(playlistDTO.StartLocationName, playlistDTO.DestinationName);

            List<Track> tracks = new List<Track>();

            if(playlistDTO.UseTopTracks)
            {
                tracks.AddRange(GetTopTracks("rock", travelDuration, playlistDTO.GenrePercentage, playlistDTO.RepeatArtist).Result);
                tracks.AddRange(GetTopTracks("metal", travelDuration, playlistDTO.GenrePercentage, playlistDTO.RepeatArtist).Result);
                tracks.AddRange(GetTopTracks("pop", travelDuration, playlistDTO.GenrePercentage, playlistDTO.RepeatArtist).Result);
                tracks.AddRange(GetTopTracks("jazz", travelDuration, playlistDTO.GenrePercentage, playlistDTO.RepeatArtist).Result);
            }
            else
            {
                tracks.AddRange(GetTracks("rock", travelDuration, playlistDTO.GenrePercentage, playlistDTO.RepeatArtist).Result);
                tracks.AddRange(GetTracks("metal", travelDuration, playlistDTO.GenrePercentage, playlistDTO.RepeatArtist).Result);
                tracks.AddRange(GetTracks("pop", travelDuration, playlistDTO.GenrePercentage, playlistDTO.RepeatArtist).Result);
                tracks.AddRange(GetTracks("jazz", travelDuration, playlistDTO.GenrePercentage, playlistDTO.RepeatArtist).Result);
            }

            List<Track> finalPlaylist = FinetunePlaytime(travelDuration, tracks, playlistDTO.GenrePercentage).ToList();
            double playtime = CalculatePlaytime(finalPlaylist);

            var playlist = new Playlist()
            {
                User = playlistDTO.User,
                UserId = playlistDTO.UserId,
                Title = playlistDTO.PlaylistName,
                UseTopTracks = playlistDTO.UseTopTracks,
                RepeatArtist = playlistDTO.RepeatArtist,
                StartLocation = playlistDTO.StartLocationName,
                Destination = playlistDTO.DestinationName,
                TravelDuration = travelDuration,
                PlaylistPlaytime = playtime,
                PlaytimeString = GetPlaytimeString((int)playtime),
                Rank = CalculateRank(finalPlaylist),
                CreatedOn = this.dateTimeProvider.GetDateTime()
            };
            
            await this.context.Playlists.AddAsync(playlist);
            await this.context.SaveChangesAsync();

            var playlistFromDb = this.context.Playlists.FirstOrDefault(x => x.Title == playlist.Title);
            if(playlistFromDb == null)
            {
                throw new ArgumentNullException("Playlist not found in the database.");
            }

            List<PlaylistTrack> playlistTracks = finalPlaylist.Select(x => new PlaylistTrack(x.Id, playlistFromDb.Id)).ToList();
            playlistFromDb.TracksCount = playlistTracks.Count;

            List<string> genresStringList = playlistDTO.GenrePercentage.Where(x => x.Value > 0).Select(y => y.Key).ToList();

            List<Genre> genres = this.context.Genres.Where(x => genresStringList.Contains(x.Name)).ToList();

            List<PlaylistGenre> playlistGenres = genres.Select(x => new PlaylistGenre(x.Id, playlistFromDb.Id)).ToList();
            playlistFromDb.GenresCount = genres.Count;

            await this.context.PlaylistTracks.AddRangeAsync(playlistTracks);
            await this.context.PlaylistGenres.AddRangeAsync(playlistGenres);
            await this.context.SaveChangesAsync();

            return new PlaylistDTO(playlist);
        }
    }
}
