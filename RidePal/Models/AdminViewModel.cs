using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;
using RidePal.Service.DTO;

namespace RidePal.Models
{
    public class AdminViewModel
    {
        private readonly IList<UserDTO> allUsers;

        public AdminViewModel()
        {
            this.allUsers = new List<UserDTO>();
        }

        public IList<UserDTO> AllUsers { get; set; }
    }
}
