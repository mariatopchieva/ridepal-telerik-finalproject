using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Service.DTO
{
    public class ArtistDTO
    {
        public ArtistDTO(Artist artist)
        {
            this.Id = artist.Id;
            this.ArtistName = artist.ArtistName;
            this.ArtistTracklistURL = artist.ArtistTracklistURL;
            this.ArtistPictureURL = artist.ArtistPictureURL;
        }

        public ArtistDTO()
        {
        }

        public long Id { get; set; }
        public string ArtistName { get; set; }
        public string ArtistTracklistURL { get; set; }
        public string ArtistPictureURL { get; set; }
    }
}
