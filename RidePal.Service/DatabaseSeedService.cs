using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Data.Models.DeezerAPIModels;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;
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
        private readonly IGeneratePlaylistService service;

        public DatabaseSeedService(RidePalDbContext _context, IGeneratePlaylistService _service)
        {
            this.context = _context;
            this.service = _service;
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

            while (currentTracklistUrl != null)
            {
                if (Tracks.Count >= trackCountBeforeSeed + 250) //250 tracks with each genre
                {
                    break;
                }

                var response = await client.GetAsync(currentTracklistUrl);

                var jsonString = await response.Content.ReadAsStringAsync();

                var deezerTrackCollection = JsonSerializer.Deserialize<DeezerTrackCollection>(jsonString);

                foreach (var track in deezerTrackCollection.Tracks)
                {
                    if (Tracks.ContainsKey(track.Id))
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

        public IEnumerable<GeneratePlaylistDTO> GeneratePlaylists()
        {
            return new GeneratePlaylistDTO[]
            {
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Sofia",
                    DestinationName = "Ihtiman",
                    PlaylistName = "Ihtiman",
                    RepeatArtist = true,
                    UseTopTracks = true,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 20
                        },
                        {
                            "metal", 40
                        },
                        {
                            "pop", 20
                        },
                        {
                            "jazz", 20
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Sofia",
                    DestinationName = "Varna",
                    PlaylistName = "To the sea",
                    RepeatArtist = false,
                    UseTopTracks = true,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 0
                        },
                        {
                            "metal", 60
                        },
                        {
                            "pop", 40
                        },
                        {
                            "jazz", 0
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Sofia",
                    DestinationName = "Plovdiv",
                    PlaylistName = "Plovdiv",
                    RepeatArtist = true,
                    UseTopTracks = false,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 100
                        },
                        {
                            "metal", 0
                        },
                        {
                            "pop", 0
                        },
                        {
                            "jazz", 0
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Sofia",
                    DestinationName = "Harmanli",
                    PlaylistName = "Harmanli",
                    RepeatArtist = false,
                    UseTopTracks = false,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 10
                        },
                        {
                            "metal", 20
                        },
                        {
                            "pop", 50
                        },
                        {
                            "jazz", 20
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Targovishte",
                    DestinationName = "Plovdiv",
                    PlaylistName = "Targovishte Plovdiv",
                    RepeatArtist = false,
                    UseTopTracks = true,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 0
                        },
                        {
                            "metal", 0
                        },
                        {
                            "pop", 0
                        },
                        {
                            "jazz", 100
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Kardzhali",
                    DestinationName = "Plovdiv",
                    PlaylistName = "Kardzhali Plovdiv",
                    RepeatArtist = true,
                    UseTopTracks = true,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 20
                        },
                        {
                            "metal", 80
                        },
                        {
                            "pop", 0
                        },
                        {
                            "jazz", 0
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Sofia",
                    DestinationName = "Svoge",
                    PlaylistName = "Svoge",
                    RepeatArtist = false,
                    UseTopTracks = true,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 40
                        },
                        {
                            "metal", 20
                        },
                        {
                            "pop", 10
                        },
                        {
                            "jazz", 30
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Sofia",
                    DestinationName = "Lukovit",
                    PlaylistName = "Lukovit",
                    RepeatArtist = true,
                    UseTopTracks = true,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 50
                        },
                        {
                            "metal", 50
                        },
                        {
                            "pop", 0
                        },
                        {
                            "jazz", 0
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Sofia",
                    DestinationName = "Svilengrad",
                    PlaylistName = "Svilengrad",
                    RepeatArtist = false,
                    UseTopTracks = false,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 0
                        },
                        {
                            "metal", 0
                        },
                        {
                            "pop", 100
                        },
                        {
                            "jazz", 0
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Sofia",
                    DestinationName = "Samokov",
                    PlaylistName = "Samokov",
                    RepeatArtist = true,
                    UseTopTracks = false,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 0
                        },
                        {
                            "metal", 0
                        },
                        {
                            "pop", 50
                        },
                        {
                            "jazz", 50
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Burgas",
                    DestinationName = "Varna",
                    PlaylistName = "Burgas Varna",
                    RepeatArtist = false,
                    UseTopTracks = true,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 20
                        },
                        {
                            "metal", 20
                        },
                        {
                            "pop", 0
                        },
                        {
                            "jazz", 60
                        }

                    },
                    UserId = 2
                },
                new GeneratePlaylistDTO()
                {
                    StartLocationName = "Varna",
                    DestinationName = "Burgas",
                    PlaylistName = "Varna Burgas",
                    RepeatArtist = true,
                    UseTopTracks = false,
                    GenrePercentage = new Dictionary<string, int>()
                    {
                        {
                            "rock", 50
                        },
                        {
                            "metal", 10
                        },
                        {
                            "pop", 30
                        },
                        {
                            "jazz", 10
                        }

                    },
                    UserId = 2
                }
            };
        }
    }
}
