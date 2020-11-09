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
        private readonly DatabaseSeedService seedService;
        private readonly IGeneratePlaylistService playlistService;

        public HomeController(ILogger<HomeController> logger, DatabaseSeedService seedService, IGeneratePlaylistService _playlistService)
        {
            _logger = logger;
            this.seedService = seedService;
            playlistService = _playlistService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SeedDatabase()
        {
            await seedService.DownloadTrackData("rock");
            await seedService.DownloadTrackData("metal");
            await seedService.DownloadTrackData("pop");
            await seedService.DownloadTrackData("jazz");

            return View("Index");
        }

        public async Task<IActionResult> GetTravelDuration()
        {
            var result = await playlistService.GetTravelDuration("Sofia", "Varna");
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
