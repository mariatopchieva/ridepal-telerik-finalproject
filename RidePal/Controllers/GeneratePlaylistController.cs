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
        private IPlaylistService playlistService;


        public GeneratePlaylistController(IGeneratePlaylistService _generatePlaylistService, IPlaylistService _playlistService)
        {
            this.generatePlaylistService = _generatePlaylistService;
            this.playlistService = _playlistService;
        }

        public IActionResult Index()
        {
            return View("GeneratePlaylist");
        }



        public async Task<IActionResult> Test()
        {
            //var generatePlaylistDTO = new GeneratePlaylistDTO()
            //{
            //    StartLocationName = "Varna",
            //    DestinationName = "Burgas",
            //    PlaylistName = "Varna Burgas7",
            //    RepeatArtist = true,
            //    UseTopTracks = false,
            //    GenrePercentage = new Dictionary<string, int>()
            //    {
            //        {
            //            "rock", 50
            //        },
            //        {
            //            "metal", 10
            //        },
            //        {
            //            "pop", 30
            //        },
            //        {
            //            "jazz", 10
            //        }

            //    },
            //    UserId = 2
            //};

            //var playlist = generatePlaylistService.GeneratePlaylist(generatePlaylistDTO).Result;

            ////// next test
            var playlistDTO = new EditPlaylistDTO()
            {
                Title = "Varna Burgas7",
                Id = 14,
                GenrePercentage = new Dictionary<string, int>()
                {
                    {
                        "rock", 50
                    },
                    {
                        "metal", 0
                    },
                    {
                        "pop", 50
                    },
                    {
                        "jazz", 0
                    }

                },
                UserId = 2
            };

            //var playlist = playlistService.EditPlaylistAsync(playlistDTO).Result;

            return View("Index");
        }

    }
}
