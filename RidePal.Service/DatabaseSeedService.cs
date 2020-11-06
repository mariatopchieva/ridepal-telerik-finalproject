using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Data.Models.DeezerAPIModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RidePal.Service
{
    public class DatabaseSeedService
    {
        private readonly RidePalDbContext context;
        HttpClient client = new HttpClient();
        Dictionary<long, Artist> Artists = new Dictionary<long, Artist>();
        Dictionary<long, Album> Albums = new Dictionary<long, Album>();
        Dictionary<long, Track> Tracks = new Dictionary<long, Track>();
        Genre genre = new Genre();

        public DatabaseSeedService(RidePalDbContext _context)
        {
            context = _context;
        }

        public async Task DownloadTrackData(string incomingGenre)
        {
            string startUrl = $"http://api.deezer.com/search/playlist?q=rock";

            genre.Name = "rock";

            var response = await client.GetAsync(startUrl);

            var jsonString = await response.Content.ReadAsStringAsync();

            var deezerPlaylistCollection = JsonSerializer.Deserialize<DeezerPlaylistCollection>(jsonString);

            foreach (var item in deezerPlaylistCollection.DeezerPlaylists)
            {
                await TraverseTracklist(item.TracklistUrl);
            }

            await context.SaveChangesAsync();
        }


        public async Task TraverseTracklist(string currentTracklistUrl)
        {

            while(currentTracklistUrl != null)
            {
                if(Tracks.Count >= 1001)
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

                    context.Tracks.Add(track);

                }

                currentTracklistUrl = deezerTrackCollection.NextPageUrl;
            }
        }

    }



}
