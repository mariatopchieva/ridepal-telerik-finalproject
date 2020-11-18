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

        public HomeController(ILogger<HomeController> logger, IDatabaseSeedService seedService, IGeneratePlaylistService generatePlaylistService)
        {
            this._logger = logger;
            this._seedService = seedService;
            this._generatePlaylistService = generatePlaylistService;
        }

        public IActionResult Index()
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

        public async Task<IActionResult> GetTravelDuration()
        {
            var result = await _generatePlaylistService.GetTravelDuration("Sofia", "Varna");
            return new JsonResult(result);
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
    }
}
