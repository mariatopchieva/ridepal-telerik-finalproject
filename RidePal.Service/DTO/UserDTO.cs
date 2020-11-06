using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RidePal.Service.DTO
{
    public class UserDTO
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [DisplayName("Username")]
        public string UserName { get; set; }
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
