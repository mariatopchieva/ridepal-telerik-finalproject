using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RidePal.Controllers
{
    [Route("/[controller]")]

    public class GeneratePlaylistController : Controller
    {
        public GeneratePlaylistController()
        {

        }
        
        public IActionResult Index()
        {
            return View("GeneratePlaylist");
        }
    }
}
