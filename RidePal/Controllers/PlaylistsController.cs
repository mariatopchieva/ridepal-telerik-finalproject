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
        public async Task<IActionResult> Index(int currentPage = 1)
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
                    StartLocation = genPlView.StartLocationName,
                    Destination = genPlView.DestinationName,
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
        [HttpGet("/Edit")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Edit(int? playlistId)
        {
            if (playlistId == null)
            {
                throw new ArgumentNullException("No playlist ID provided.");
            }

            var playlistDTO = await this.service.GetPlaylistByIdAsync(playlistId.Value);

            if (playlistDTO == null)
            {
                throw new ArgumentNullException("No such playlist was found in the database.");
            }

            return View(new PlaylistViewModel(playlistDTO));
        }

        // POST: PlaylistsController/Edit/5
        [HttpPost("/Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int playlistId, [Bind("Id,Title,GenrePercentage")] EditPlaylistDTO editedPlaylist)
        {
            //genrePercentage e Dictionary => ako го сменим с List<string>, трябва да edit-нем EditPlaylist() в PlaylistService
            
            var userId = int.Parse(userManager.GetUserId(HttpContext.User));

            if (playlistId != editedPlaylist.Id)
            {
                throw new ArgumentException("Playlist ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var playlistDTO = this.service.EditPlaylistAsync(editedPlaylist).Result;
                }
                catch
                {
                    throw new ArgumentException("The playlist was not edited.");
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PlaylistsController/Delete/5
        [HttpGet("/Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? playlistId)
        {
            if (playlistId == null)
            {
                throw new ArgumentNullException("No playlist ID provided.");
            }

            var playlist = await this.service.GetPlaylistByIdAsync(playlistId.Value);

            if (playlist == null)
            {
                throw new ArgumentNullException("No such playlist was found in the database.");
            }

            return View(new PlaylistViewModel(playlist));
        }

        // POST: PlaylistsController/Delete/5
        [HttpPost("/DeleteConfirmed"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int playlistId)
        {
            var playlist = await this.service.DeletePlaylistAsync(playlistId);
            return RedirectToAction(nameof(Index));
        }
    }
}
