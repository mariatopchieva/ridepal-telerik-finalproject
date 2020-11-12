using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;

namespace RidePal.Controllers
{
    [Route("/[controller]")]

    public class GeneratePlaylistController : Controller
    {
        private IGeneratePlaylistService generatePlaylistService;

        public GeneratePlaylistController(IGeneratePlaylistService _generatePlaylistService)
        {
            this.generatePlaylistService = _generatePlaylistService;
        }
        //public IActionResult Index()
        //{
        //    return View("GeneratePlaylist");
        //}


        public IActionResult Index()
        {
            var duration = generatePlaylistService.GetTravelDuration("Sofia", "Varna").Result;

            Dictionary<string, int> genrePercentage = new Dictionary<string, int>() 
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
                },

            };

            GeneratePlaylistDTO playlistDTO = new GeneratePlaylistDTO()
            {
                StartLocationName = "Sofia",
                DestinationName = "Ihtiman",
                PlaylistName = "To the sea",
                RepeatArtist = false,
                UseTopTracks = false,
                GenrePercentage = genrePercentage,
                User = new Data.Models.User()
            };


            var playlist = generatePlaylistService.GeneratePlaylist(playlistDTO).Result;

            return View("GeneratePlaylist");
        }



    }
}
