﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RidePal.Models;
using RidePal.Service;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;

namespace RidePal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseSeedService _seedService;
        private readonly IGeneratePlaylistService _generatePlaylistService;
        private readonly IPlaylistService _playlistService;
        private readonly IStatisticsService _statistics;
        private readonly IPixaBayImageService _imageService;

        public HomeController(ILogger<HomeController> logger, 
                                IDatabaseSeedService seedService, 
                                IGeneratePlaylistService generatePlaylistService,
                                IPlaylistService playlistService,
                                IStatisticsService stats,
                                IPixaBayImageService imageService)
        {
            this._logger = logger;
            this._seedService = seedService;
            this._generatePlaylistService = generatePlaylistService;
            this._playlistService = playlistService;
            this._statistics = stats;
            this._imageService = imageService;
        }

        public IActionResult Index()
        {
            var homeView = new HomeViewModel()
            {
                TrackCount = this._statistics.TrackCount().Result,
                ArtistCount = this._statistics.ArtistCount().Result,
                GenreCount = this._statistics.GenreCount().Result,
                PlaylistCount = this._statistics.PlaylistCount().Result,
                UserCount = this._statistics.UserCount().Result,
                TopPlaylists = this._statistics.TopPlaylists().Result.Select(x => new PlaylistViewModel(x)).ToList(),
                FeaturedArtists = this._statistics.FeaturedArtists().Result,
            };

            return View(homeView);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        public async Task<IActionResult> SeedDatabase()
        {
            await _seedService.DownloadTrackData("rock");
            await _seedService.DownloadTrackData("metal");
            await _seedService.DownloadTrackData("pop");
            await _seedService.DownloadTrackData("jazz");

            return View("Index");
        }

        public IActionResult SeedPlaylists()
        {
            var generatePlaylistsDTO = _seedService.GeneratePlaylists();

            foreach (var generatePlaylistDTO in generatePlaylistsDTO)
            {
                var playlist = _generatePlaylistService.GeneratePlaylist(generatePlaylistDTO).Result;
            }

            return View("Index");
        }

        public IActionResult AttachImages()
        {
            var playlistDTO = this._playlistService.GetPlaylistByIdAsync(139).Result;

            var playlistDTOWithImg = this._playlistService.AttachImage(playlistDTO);

            return View("Index");
        }

        public async Task<IActionResult> Test()
        {
            //var add = await this._playlistService.AddPlaylistToFavoritesAsync(90, 1);
            //var remove = await this._playlistService.RemovePlaylistFromFavoritesAsync(90, 1);
            //var addAgain = await this._playlistService.AddPlaylistToFavoritesAsync(90, 1);

            //List<string> genres = new List<string>() { "jazz" };
            //List<int> durations = new List<int> { 0, 18767 };
            //var playlists = await this._playlistService.GetAllPlaylistsAsync();
            //var playlist1 = this._playlistService.FilterPlaylistsByName("Plovdiv", playlists);
            //var playlist2 = await this._playlistService.FilterPlaylistsByGenreAsync(genres, playlist1);
            //var playlist3 = this._playlistService.FilterPlaylistsByDuration(durations, playlist2);
            //var playlistsMaster = await this._playlistService.FilterPlaylistsMasterAsync("Plovdiv", genres, durations);

            //    public int Id { get; set; }
            //public string Title { get; set; }
            //public Dictionary<string, int> GenrePercentage { get; set; }
            //public User User { get; set; }
            //public int UserId { get; set; }

            //var editPlaylistDTO = new EditPlaylistDTO
            //{
            //    Id = 88,
            //    Title = "To the mountain",
            //    GenrePercentage = new Dictionary<string, int> { { "pop", 50 }, { "jazz", 50 } },
            //    UserId = 2
            //};

            //var playlist = await this._playlistService.EditPlaylistAsync(editPlaylistDTO);

            var playlist = new GeneratePlaylistDTO()
            {
                StartLocation = "Sofia",
                Destination = "Ihtiman",
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
            };

            var returnedPlaylist = await this._generatePlaylistService.GeneratePlaylist(playlist);

            return RedirectToAction(nameof(Index));
        }
    }
}
