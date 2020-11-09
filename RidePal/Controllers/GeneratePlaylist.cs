using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RidePal.Controllers
{
    [Route("/[controller]")]

    public class GeneratePlaylist : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
