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
            var generatePlaylistDTO = new GeneratePlaylistDTO()
            {
                StartLocationName = "Varna",
                DestinationName = "Burgas",
                PlaylistName = "Varna Burgas6",
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
            };

             var playlist = generatePlaylistService.GeneratePlaylist(generatePlaylistDTO).Result;

            return View("GeneratePlaylist");
        }



    }
}
