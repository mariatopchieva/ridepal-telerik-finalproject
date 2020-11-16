using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RidePal.Data.Models
{
    public class User : IdentityUser<int>
    {
        [/*Required, */MinLength(3), MaxLength(50)]
        public string FirstName { get; set; }

        [/*Required,*/ MinLength(3), MaxLength(50)]
        public string LastName { get; set; }

        public bool IsBanned { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public ICollection<Playlist> Playlists { get; set; }

        public ICollection<PlaylistFavorite> Favorites { get; set; }

    }
}
