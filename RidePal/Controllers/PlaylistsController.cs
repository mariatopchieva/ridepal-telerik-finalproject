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
        private readonly IPixaBayImageService imageService;

        public PlaylistsController(IPlaylistService _service, IGeneratePlaylistService _generateService,
                                    IAdminService _adminService, UserManager<User> _userManager,
                                    IPixaBayImageService imgService)
        {
            this.service = _service;
            this.generateService = _generateService;
            this.adminService = _adminService;
            this.userManager = _userManager;
            this.imageService = imgService;
        }

        // GET: PlaylistsController
        [HttpGet("/Index")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PlaylistDTO> playlistsDTO = await service.GetAllPlaylistsAsync();

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
                MaxDuration = service.GetHighestPlaytimeAsync().Result
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

                var playlistsDTO = await service.FilterPlaylistsMasterAsync(filteredName, filteredGenres, filteredDuration);

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
                    MaxDuration = service.GetHighestPlaytimeAsync().Result
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

            //var playlistView 

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
        public async Task<IActionResult> Create([Bind("Title," +
                                                    "StartLocationName, DestinationName," +
                                                    "RepeatArtist, TopTracks," +
                                                    "MetalPercentage, RockPercentage, PopPercentage, JazzPercentage")]
                                                    GeneratePlaylistViewModel genPlView)
        {
            var userId = int.Parse(userManager.GetUserId(HttpContext.User));

            var genres = new Dictionary<string, int>();
            genres.Add("metal", genPlView.MetalPercentage);
            genres.Add("rock", genPlView.RockPercentage);
            genres.Add("pop", genPlView.PopPercentage);
            genres.Add("jazz", genPlView.JazzPercentage);

            if (ModelState.IsValid)
            {
                var genPlaylistDTO = new GeneratePlaylistDTO()
                {
                    PlaylistName = genPlView.Title,
                    StartLocationName = genPlView.StartLocationName,
                    DestinationName = genPlView.DestinationName,
                    RepeatArtist = genPlView.RepeatArtist,
                    UseTopTracks = genPlView.TopTracks,
                    GenrePercentage = genres,
                    UserId = userId
                };

                PlaylistDTO playlistDTO = await generateService.GeneratePlaylist(genPlaylistDTO);

                if (playlistDTO == null)
                {
                    throw new ArgumentNullException("Something went wrong with playlist creation.");
                }

                var playlistDtoWithImg = await this.service.AttachImage(playlistDTO);


                return RedirectToAction("Details", new { id = playlistDtoWithImg.Id});
            }

            return RedirectToAction("Index", new { error = TempData["Error"] = "Playlist creation failed." });
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
