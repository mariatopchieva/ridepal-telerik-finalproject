using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RidePal.Data.Models;
using RidePal.Models;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;

namespace RidePal.Controllers
{
    [Route("/[controller]")]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistService service;
        private readonly IGeneratePlaylistService generateService;
        private readonly IAdminService adminService;
        private readonly UserManager<User> userManager;

        public PlaylistsController(IPlaylistService _service, IGeneratePlaylistService _generateService,
            IAdminService _adminService, UserManager<User> _userManager)
        {
            this.service = _service;
            this.generateService = _generateService;
            this.adminService = _adminService;
            this.userManager = _userManager;
        }

        // GET: PlaylistsController
        [HttpGet("/Index")]
        public async Task<IActionResult> Index([FromQuery] int currentPage = 1)
        {
            IEnumerable<PlaylistDTO> playlistsDTO = service.GetPlaylistsPerPage(currentPage);

            if (playlistsDTO == null)
            {
                return NotFound();
            }

            var playlistsViewModels = new List<PlaylistViewModel>();
            foreach (var playlist in playlistsDTO)
            {
                var currentPlaylistViewModel = new PlaylistViewModel(playlist);
                playlistsViewModels.Add(currentPlaylistViewModel);
            }

            FilteredPlaylistsViewModel filteredPlaylistList = new FilteredPlaylistsViewModel()
            {
                Playlists = playlistsViewModels,
                AllGenres = service.GetAllGenresAsync().Result.OrderBy(x => x.Name).ToList(),
                MaxDuration = service.GetHighestPlaytimeAsync().Result,
                TotalPages = service.GetPageCount(),
                CurrentPage = currentPage
            };

            return View(filteredPlaylistList);
        }

        // POST: PlaylistsController
        [HttpPost("/Index")]
        public async Task<IActionResult> Index([Bind("Name,GenresNames,DurationLimits")] FilterCriteria filterCriteria)
        {
            if (ModelState.IsValid)
            {
                var filteredName = filterCriteria.Name;
                var filteredGenres = filterCriteria.GenresNames;
                var filteredDuration = filterCriteria.DurationLimits;
                List<string> genres = new List<string>();

                if(filteredGenres[0] == "true") //трябва да се върже с GetAllGenres, понеже ако се увеличат, тук филтрирам само 4
                {
                    genres.Add("jazz");
                }

                if (filteredGenres[1] == "true")
                {
                    genres.Add("metal");
                }

                if (filteredGenres[2] == "true")
                {
                    genres.Add("pop");
                }

                if (filteredGenres[3] == "true")
                {
                    genres.Add("rock");
                }

                //temporary until the slider range gets two values
                filteredDuration.Insert(0, 0);

                var playlistsDTO = await service.FilterPlaylistsMasterAsync(filteredName, genres, filteredDuration);

                if (playlistsDTO == null)
                {
                    return NotFound();
                }

                var playlistsViewModels = new List<PlaylistViewModel>();
                foreach (var playlist in playlistsDTO)
                {
                    var currentPlaylistViewModel = new PlaylistViewModel(playlist);
                    playlistsViewModels.Add(currentPlaylistViewModel);
                }

                FilteredPlaylistsViewModel filteredPlaylistList = new FilteredPlaylistsViewModel()
                {
                    Playlists = playlistsViewModels,
                    AllGenres = service.GetAllGenresAsync().Result.OrderBy(x => x.Name).ToList(),
                    MaxDuration = service.GetHighestPlaytimeAsync().Result //what is the default max value?
                };

                return View(filteredPlaylistList);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PlaylistsController/Details/5
        [HttpGet("/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var playlist = await service.GetPlaylistByIdAsync(id.Value);
            
            if (playlist == null)
            {
                return NotFound();
            }

            return View("Details");
        }

        // GET: PlaylistsController/Create
        [HttpGet("/Create")]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlaylistsController/Create
        [HttpPost("/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistName,StartLocationName,DestinationName,RepeatArtist,UseTopTracks,GenrePercentage,UserId")] 
        GeneratePlaylistDTO generatePlaylistDTO)
        {
            if (ModelState.IsValid)
            {
                PlaylistDTO playlistDTO = await generateService.GeneratePlaylist(generatePlaylistDTO);
                return RedirectToAction("Index", new { msg = TempData["Msg"] = "Playlist created." });
                //return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", new { error = TempData["Error"] = "Playlist creation failed." });
            //return View();
        }

        // GET: PlaylistsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlaylistsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlaylistsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlaylistsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        
    }
}
