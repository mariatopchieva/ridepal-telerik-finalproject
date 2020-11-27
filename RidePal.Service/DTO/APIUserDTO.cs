using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RidePal.Service.DTO
{
    public class APIUserDTO
    {
        public APIUserDTO(User user)
        {
            this.Id = user.Id;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.UserName = user.UserName;
            //this.Password = user.PasswordHash;
            this.Token = user.Token;
        }

        public APIUserDTO()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
