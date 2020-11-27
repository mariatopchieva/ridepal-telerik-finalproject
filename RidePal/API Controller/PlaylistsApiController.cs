using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;

namespace RidePal
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsApiController : ControllerBase
    {
        private readonly IAPIUserService userService;
        private readonly UserManager<User> userManager; //?? which class
        private readonly IPlaylistService playlistService;
        private readonly IGeneratePlaylistService generateService;
        private readonly IPixaBayImageService imageService;

        public PlaylistsApiController(IAPIUserService _userService, UserManager<User> _userManager,
                                  IPlaylistService _playlistService, IGeneratePlaylistService _generateService,
                                  IPixaBayImageService _imageService)
        {
            this.playlistService = _playlistService;
            this.generateService = _generateService;
            this.imageService = _imageService;
            this.userService = _userService;
            this.userManager = _userManager;
        }

        // GetAll / GetById / GeneratePlaylist / EditPlaylist / DeletePlaylist
        //GET api/playlistsapi
        [HttpGet("")]
        public IActionResult GetAllPlaylists()
        {
            var playlists = this.playlistService.GetAllPlaylistsAsync().Result;

            if (playlists == null)
            {
                return NotFound();
            }

            return Ok(playlists);
        }

        //GET api/playlistsapi/:id
        [HttpGet("{id}")]
        public IActionResult GetPlaylistById(int id)
        {
            var playlist = this.playlistService.GetPlaylistByIdAsync(id).Result;

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        //POST api/playlistsapi
        [HttpPost("")]
        public IActionResult CreatePlaylist([FromBody] GeneratePlaylistDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var userId = int.Parse(userManager.GetUserId(HttpContext.User));

            var generatePaylistDTO = new GeneratePlaylistDTO
            {
                StartLocation = model.StartLocation,
                Destination = model.Destination,
                PlaylistName = model.PlaylistName,
                RepeatArtist = model.RepeatArtist,
                UseTopTracks = model.UseTopTracks,
                GenrePercentage = model.GenrePercentage,
                UserId = userId
            };

            var playlist = this.generateService.GeneratePlaylist(generatePaylistDTO).Result;

            if (playlist == null)
            {
                return BadRequest();
            }

            var playlistWithImage = this.playlistService.AttachImage(playlist).Result;

            return Created("post", playlistWithImage);
        }

        //PUT api/playlistsapi/:id
        [HttpPut("{id}")]
        public IActionResult UpdatePlaylist(int id, [FromBody] EditPlaylistDTO model)
        {
            if (id < 1 || model == null)
            {
                return BadRequest();
            }

            var userId = int.Parse(userManager.GetUserId(HttpContext.User));

            var playlist = new EditPlaylistDTO
            {
                Id = id,
                Title = model.Title,
                GenrePercentage = model.GenrePercentage,
                UserId = userId
            };

            var result = this.playlistService.EditPlaylistAsync(playlist).Result;

            if (result == false)
            {
                return BadRequest();
            }

            return Ok();
        }

        //DELETE api/playlistsapi/:id
        [HttpDelete("{id}")]
        public IActionResult DeletePlaylist(int id)
        {
            var result = this.playlistService.DeletePlaylistAsync(id).Result;

            if (result == true)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
