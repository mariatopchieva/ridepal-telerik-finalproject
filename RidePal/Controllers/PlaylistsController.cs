using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            FilteredPlaylistsViewModel filteredPlaylistList = new FilteredPlaylistsViewModel();
            filteredPlaylistList.Playlists = playlistsViewModels;

            //ViewData["Countries"] = new SelectList(_countryService.GetAllCountries().OrderBy(x => x.Name), "Id", "Name");
            //ViewData["Styles"] = new SelectList(_styleService.GetAllStyles().Result.OrderBy(x => x.Name), "Id", "Name");

            return View(filteredPlaylistList);
        }


        // Post: Index
        //[HttpPost("/Index")]
        //public async Task<IActionResult> Index([Bind("Country,Style")] FilterCriteria filterCriteria)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var filteredCountry = filterCriteria.Country;
        //        var filteredStyle = filterCriteria.Style;
        //        IEnumerable<BeerDTO> beers = await _service.FilterBeersWebAsync(filteredCountry, filteredStyle);
        //        BeerFilterList beerFilterList = new BeerFilterList
        //        {
        //            ListBeers = beers,
        //            FilterCriteria = filterCriteria
        //        };

        //        ViewData["Countries"] = new SelectList(_countryService.GetAllCountries().OrderBy(x => x.Name), "Id", "Name");
        //        ViewData["Styles"] = new SelectList(_styleService.GetAllStyles().Result.OrderBy(x => x.Name), "Id", "Name");

        //        return View(beerFilterList);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}


        // GET: PlaylistsController/Details/5
        [HttpGet("/Details/{id}")]
        public async Task<IActionResult> Details(long? id)
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
