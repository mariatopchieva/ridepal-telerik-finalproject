using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RidePal.Models;
using RidePal.Service.Contracts;

namespace RidePal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IDatabaseSeedService _seedService;
        private readonly IGeneratePlaylistService _playlistService;
        private readonly IAdminService adminService;

        public AdminController(ILogger<AdminController> logger, 
                                IDatabaseSeedService seedService, 
                                IGeneratePlaylistService playlistService, 
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

            var adminView = new AdminViewModel() { RegularUsers = users };

            return View(adminView);
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
    }
}
