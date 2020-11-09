using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Data.Models.DeezerAPIModels;
using RidePal.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RidePal.Service
{
    public class DatabaseSeedService : IDatabaseSeedService
    {
        private readonly RidePalDbContext context;
        HttpClient client = new HttpClient();
        Dictionary<long, Artist> Artists = new Dictionary<long, Artist>();
        Dictionary<long, Album> Albums = new Dictionary<long, Album>();
        Dictionary<long, Track> Tracks = new Dictionary<long, Track>();
        Dictionary<long, Genre> Genres = new Dictionary<long, Genre>();

        public DatabaseSeedService(RidePalDbContext _context)
        {
            context = _context;
        }

        public async Task DownloadTrackData(string incomingGenre)
        {
            string startUrl = $"http://api.deezer.com/search/playlist?q={incomingGenre}";

            var response = await client.GetAsync(startUrl);

            var jsonString = await response.Content.ReadAsStringAsync();

            var deezerPlaylistCollection = JsonSerializer.Deserialize<DeezerPlaylistCollection>(jsonString);

            foreach (var item in deezerPlaylistCollection.DeezerPlaylists)
            {
                await TraverseTracklist(item.TracklistUrl, incomingGenre);
            }

            await context.SaveChangesAsync();
        }


        public async Task TraverseTracklist(string currentTracklistUrl, string incomingGenre)
        {
            Genre genre = new Genre();
            genre.Name = incomingGenre;
            genre.Id = context.Genres.Count() + 1;

            int trackCountBeforeSeed = context.Tracks.Count();

            while(currentTracklistUrl != null)
            {
                if(Tracks.Count >= trackCountBeforeSeed + 250) //250 tracks with each genre
                {
                    break;
                }
                
                var response = await client.GetAsync(currentTracklistUrl);

                var jsonString = await response.Content.ReadAsStringAsync();

                var deezerTrackCollection = JsonSerializer.Deserialize<DeezerTrackCollection>(jsonString);

                foreach (var track in deezerTrackCollection.Tracks)
                {
                    if(Tracks.ContainsKey(track.Id))
                    {
                        continue;
                    }
                    else
                    {
                        Tracks.Add(track.Id, track);
                    }

                    if (Albums.ContainsKey(track.Album.Id))
                    {
                        track.Album = Albums[track.Album.Id];
                    }
                    else
                    {
                        Albums.Add(track.Album.Id, track.Album);
                    }

                    if (Artists.ContainsKey(track.Artist.Id))
                    {
                        track.Artist = Artists[track.Artist.Id];
                    }
                    else
                    {
                        Artists.Add(track.Artist.Id, track.Artist);
                    }

                    track.Genre = genre;

                    if (Genres.ContainsKey(track.Genre.Id))
                    {
                        track.Genre = Genres[track.Genre.Id];
                    }
                    else
                    {
                        Genres.Add(track.Genre.Id, track.Genre);
                    }

                    context.Tracks.Add(track);
                }

                currentTracklistUrl = deezerTrackCollection.NextPageUrl;
            }
        }
    }
}
