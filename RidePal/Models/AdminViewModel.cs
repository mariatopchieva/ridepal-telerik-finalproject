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
        private readonly IList<User> regularUsers;

        public AdminViewModel()
        {
            this.regularUsers = new List<User>();
        }

        public IList<User> RegularUsers { get; set; }
    }
}
