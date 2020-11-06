using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.Models
{
    public class UserViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public IEnumerable<UserDTO> Users { get; set; }
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
