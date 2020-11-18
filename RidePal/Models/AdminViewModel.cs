using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RidePal.Data.Models;

namespace RidePal.Models
{
    public class AdminViewModel
    {
        private readonly IList<User> allUsers;

        public AdminViewModel()
        {
            this.AllUsers = new List<User>();
        }

        public IList<User> AllUsers { get; set; }
    }
}
