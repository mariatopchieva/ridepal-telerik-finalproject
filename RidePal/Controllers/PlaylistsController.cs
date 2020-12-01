using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
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
    //[Route("/[controller]")]
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
        //[HttpGet("/Index")]
        [HttpGet]
        public IActionResult Index(int currentPage = 1)
        {

            IEnumerable<PlaylistDTO> playlistsDTO = service.GetPlaylistsPerPage(currentPage);

            if (playlistsDTO == null)
            {
                return NotFound();
            }

            var playlistsViewModels = playlistsDTO.Select(plDto => new PlaylistViewModel(plDto));

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
        //[HttpPost("/Index")]
        [HttpPost]
        public async Task<IActionResult> Index([Bind("Name,GenresNames,DurationLimits")] FilterCriteria filterCriteria)
        {
            if (ModelState.IsValid)
            {
                var filteredName = filterCriteria.Name;
                var filteredGenres = filterCriteria.GenresNames;
                var filteredDuration = filterCriteria.DurationLimits;
                List<string> genres = new List<string>();

                if (filteredGenres[0] == "true") //трябва да се върже с GetAllGenres, понеже ако се увеличат, тук филтрирам само 4
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
        // [HttpGet("/Details/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            PlaylistDTO playlist;

            if (User.IsInRole("Admin"))
            {
                playlist = await service.AdminGetPlaylistByIdAsync(id.Value);
            }
            else
            {
                playlist = await service.GetPlaylistByIdAsync(id.Value);
            }

            if (playlist == null)
            {
                return NotFound();
            }

            //var playlistView 
            var playlistView = new PlaylistViewModel(playlist);
            playlistView.TrackList = await service.GetPlaylistTracksAsync(playlist.Id);
            playlistView.GenreString = await service.GetPlaylistGenresAsStringAsync(playlist.Id);

            return View(playlistView);
        }

        // GET: PlaylistsController/Create
        //[HttpGet("/Create")]
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlaylistsController/Create
        //[HttpPost("/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title," +
                                                    "StartLocationName, DestinationName," +
                                                    "RepeatArtist, TopTracks," +
                                                    "MetalPercentage, RockPercentage, PopPercentage, JazzPercentage")]
                                                    GeneratePlaylistViewModel genPlView)
        {
            if ((genPlView.MetalPercentage + genPlView.RockPercentage +
                        genPlView.PopPercentage + genPlView.JazzPercentage) > 100)
            {
                return RedirectToAction("Create","Playlists", new { error = TempData["Error"] = "Combined genre percentage must not exceed 100%" });
            }

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


                return RedirectToAction("Details", new { id = playlistDtoWithImg.Id });
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
                return NotFound();
            }

            var playlistDTO = await this.service.GetPlaylistByIdAsync(playlistId.Value);

            if (playlistDTO == null)
            {
                return NotFound();
            }

            return View(new PlaylistViewModel(playlistDTO));
        }

        // POST: PlaylistsController/Edit/5
        [HttpPost("/Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int pId, string inputTitle, int inputMetal, int inputRock, int inputPop, int inputJazz)
        {
            //genrePercentage e Dictionary => ako го сменим с List<string>, трябва да edit-нем EditPlaylist() в PlaylistService
            var genres = new Dictionary<string, int>();
            genres.Add("metal", inputMetal);
            genres.Add("rock", inputRock);
            genres.Add("pop", inputPop);
            genres.Add("jazz", inputJazz);

            var editedPlaylist = new EditPlaylistDTO()
            {
                Id = pId,
                Title = inputTitle,
                GenrePercentage = genres
            };

            var result = this.service.EditPlaylistAsync(editedPlaylist).Result;

            if (result == false)
            {
                return RedirectToAction("Details", new { id = pId, err = TempData["Error"] = "Playlist not edited" });
            }
            return RedirectToAction("Details", new { id = pId, msg = TempData["Msg"] = "Playlist edited" });
        }

        // Post: PlaylistsController/Delete/5
        [HttpPost("/Delete/{pId}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Delete(int? pId)
        {
            if (pId == null)
            {
                return NotFound();
            }

            bool result = await this.service.DeletePlaylistAsync(pId.Value);

            if (result == true)
            {
                return RedirectToAction("Index", "Playlists", new { msg = TempData["Msg"] = "Playlist deleted." });
            }

            return RedirectToAction("Details", new { id = pId, error = TempData["Error"] = "Delete action failed." });
        }

        // Post: PlaylistsController/UndoDelete/5
        [HttpPost("/UndoDelete/{pId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UndoDelete(int? pId)
        {
            if (pId == null)
            {
                return NotFound();
            }

            bool result = await this.service.UndoDeletePlaylistAsync(pId.Value);

            if (result == true)
            {
                return RedirectToAction("Details", new { id = pId, msg = TempData["Msg"] = "Playlist delete undone." });
            }

            return RedirectToAction("Details", new { id = pId, error = TempData["Error"] = "Delete action failed." });
        }

        // POST: PlaylistsController/Delete/5
        //[HttpPost("/DeleteConfirmed"), ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int playlistId)
        //{
        //    var playlist = await this.service.DeletePlaylistAsync(playlistId);
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        public async Task<IActionResult> AddToFav(int plId)
        {
            var userId = int.Parse(userManager.GetUserId(HttpContext.User));

            bool result = await this.service.AddPlaylistToFavoritesAsync(plId, userId);

            if (result == true)
            {
                return RedirectToAction("Details", new { id = plId, msg = TempData["Msg"] = "Playlist added to favorites." });
            }

            return RedirectToAction("Details", new { id = plId, error = TempData["Error"] = "Playlist is already in your favorites list." });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFav(int plId)
        {
            var userId = int.Parse(userManager.GetUserId(HttpContext.User));

            bool result = await this.service.RemovePlaylistFromFavoritesAsync(plId, userId);

            if (result == true)
            {
                return RedirectToAction("Details", new { id = plId, msg = TempData["Msg"] = "Playlist removed from favorites." });
            }

            return RedirectToAction("Details", new { id = plId, error = TempData["Error"] = "Playlist is not in your favorites list." });
        }

        [HttpGet]
        public IActionResult Favorites(int currentPage = 1)
        {
            var userId = int.Parse(userManager.GetUserId(HttpContext.User));

            var playlistsDTO = service.GetPlaylistsPerPageOfCollection(currentPage, userId, "favorites");

            var totalPages = service.GetPageCountOfCollection(userId, "favorites");

            var playlistsViewModels = playlistsDTO.Select(x => new PlaylistViewModel(x));

            PlaylistCollectionViewModel playlistList = new PlaylistCollectionViewModel()
            {
                Playlists = playlistsViewModels,
                TotalPages = totalPages,
                CurrentPage = currentPage
            };

            return View(playlistList);
        }

        [HttpGet]
        public IActionResult UserPlaylists(int currentPage = 1)
        {
            var userId = int.Parse(userManager.GetUserId(HttpContext.User));

            var playlistsDTO = service.GetPlaylistsPerPageOfCollection(currentPage, userId, "myPlaylists");

            var totalPages = service.GetPageCountOfCollection(userId, "myPlaylists");

            var playlistsViewModels = playlistsDTO.Select(x => new PlaylistViewModel(x));

            PlaylistCollectionViewModel playlistList = new PlaylistCollectionViewModel()
            {
                Playlists = playlistsViewModels,
                TotalPages = totalPages,
                CurrentPage = currentPage
            };

            return View(playlistList);
        }
    }
}
