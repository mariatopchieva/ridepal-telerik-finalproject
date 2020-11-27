using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RidePal.Models;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;

namespace RidePal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IDatabaseSeedService _seedService;
        private readonly IPlaylistService _playlistService;
        private readonly IAdminService adminService;

        public AdminController(ILogger<AdminController> logger,
                                IDatabaseSeedService seedService,
                                IPlaylistService playlistService,
                                IAdminService adminService)
        {
            this._logger = logger;
            this._seedService = seedService;
            this._playlistService = playlistService;
            this.adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var users = await this.adminService.GetAllRegularUsers();

            var currentAdmin = users.FirstOrDefault(user => user.UserName == HttpContext.User.Identity.Name);

            if (currentAdmin == null)
            {
                throw new ArgumentNullException("User not found in the current list of users.");
            }

            users.Remove(currentAdmin);

            var adminView = new AdminViewModel() { AllUsers = users };

            return View(adminView);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(string text)
        {
            var users = await this.adminService.SearchByEmail(text);

            if (users == null)
            {
                return RedirectToAction("Index", "Admin", new { error = TempData["Error"] = "No users found." });
            }

            var adminView = new AdminViewModel() { AllUsers = users };

            return View(adminView);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await this.adminService.GetUserById((int)id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                var edited = await this.adminService.EditUser(user);

                return RedirectToAction("Index", "Admin", new { msg = TempData["Msg"] = "User info edited." });
            }

            return RedirectToAction("Index", "Admin", new { error = TempData["Error"] = "User info edit failed." });
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(int userId)
        {
            var banned = await this.adminService.BanUserById(userId);

            if (banned == true)
            {
                return RedirectToAction("Index", "Admin", new { msg = TempData["Msg"] = "User banned!" });
            }

            return RedirectToAction("Index", "Admin", new { error = TempData["Error"] = "User ban failed!" });
        }

        [HttpPost]
        public async Task<IActionResult> UnbanUser(int userId)
        {
            var banned = await this.adminService.UnbanUserById(userId);

            if (banned == true)
            {
                return RedirectToAction("Index", "Admin", new { msg = TempData["Msg"] = "User unbanned!" });
            }

            return RedirectToAction("Index", "Admin", new { error = TempData["Error"] = "User unban failed!" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var deleted = await this.adminService.DeleteUser(userId);

            if (deleted == true)
            {
                return RedirectToAction("Index", "Admin", new { msg = TempData["Msg"] = "User unbanned!" });
            }

            return RedirectToAction("Index", "Admin", new { error = TempData["Error"] = "User unban failed!" });
        }

        [HttpPost]
        public async Task<IActionResult> RevertDeleteUser(int userId)
        {
            var reverted = await this.adminService.RevertDeleteUser(userId);

            if (reverted == true)
            {
                return RedirectToAction("Index", "Admin", new { msg = TempData["Msg"] = "User unbanned!" });
            }

            return RedirectToAction("Index", "Admin", new { error = TempData["Error"] = "User unban failed!" });
        }

        [HttpGet]
        public IActionResult DeletedPlaylists(int currentPage = 1)
        {

            IEnumerable<PlaylistDTO> playlistsDTO = this.adminService.GetDeletedPlaylists().Result;

            if (playlistsDTO == null)
            {
                return NotFound();
            }

            var playlistsViewModels = playlistsDTO.Select(plDto => new PlaylistViewModel(plDto));

            FilteredPlaylistsViewModel filteredPlaylistList = new FilteredPlaylistsViewModel()
            {
                Playlists = playlistsViewModels,
                AllGenres = _playlistService.GetAllGenresAsync().Result.OrderBy(x => x.Name).ToList(),
                MaxDuration = _playlistService.GetHighestPlaytimeAsync().Result,
                TotalPages = _playlistService.GetPageCount(),
                CurrentPage = currentPage
            };

            return View(filteredPlaylistList);
        }
    }
}
