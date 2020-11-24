using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RidePal.Models;
using RidePal.Service;
using RidePal.Service.Contracts;

namespace RidePal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseSeedService _seedService;
        private readonly IGeneratePlaylistService _generatePlaylistService;
        private readonly IPlaylistService _playlistService;
        private readonly IStatisticsService _statistics;

        public HomeController(ILogger<HomeController> logger, 
                                IDatabaseSeedService seedService, 
                                IGeneratePlaylistService generatePlaylistService,
                                IPlaylistService playlistService,
                                IStatisticsService stats)
        {
            this._logger = logger;
            this._seedService = seedService;
            this._generatePlaylistService = generatePlaylistService;
            this._playlistService = playlistService;
            this._statistics = stats;
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

        public async Task<IActionResult> Test()
        {
            List<string> genres = new List<string> () { "jazz" };
            List<int> durations = new List<int> { 0, 18767 };
            var playlists = await this._playlistService.GetAllPlaylistsAsync();
            var playlist1 = this._playlistService.FilterPlaylistsByName("Plovdiv", playlists);
            var playlist2 = await this._playlistService.FilterPlaylistsByGenreAsync(genres, playlist1);
            var playlist3 = this._playlistService.FilterPlaylistsByDuration(durations, playlist2);

            var playlistsMaster = await this._playlistService.FilterPlaylistsMasterAsync("Plovdiv", genres, durations);

            return RedirectToAction(nameof(Index));
        }
    }
}
