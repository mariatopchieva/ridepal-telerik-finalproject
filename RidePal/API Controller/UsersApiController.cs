using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Service.ApiHelpers;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;

namespace RidePal.API_Controller
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersApiController : ControllerBase
    {
        private readonly IAPIUserService userService;
        private readonly UserManager<User> userManager; //?? which class

        public UsersApiController(IAPIUserService _userService, UserManager<User> _userManager)
        {
            this.userService = _userService;
            this.userManager = _userManager;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] LoginCredentialsModel model)
        {
            var user = this.userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(user);
        }
    }
}
