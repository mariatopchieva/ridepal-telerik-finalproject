using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.DTO
{
    public class EditPlaylistDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Dictionary<string, int> GenrePercentage { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}
