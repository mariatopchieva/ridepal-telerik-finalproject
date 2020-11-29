using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using RidePal.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.AdminServiceTests
{
    [TestClass]
    public class FeaturedArtists_Should
    {
        [TestMethod]
        public async Task ReturnThreeArthistsFromDB_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnThreeArthistsFromDB_Correctly));
            var plMock = new Mock<IPlaylistService>();

            var artists = new List<Artist>()
            {
                new Artist()
                {
                Id = 1,
                ArtistName = "The weeknd",
                ArtistPictureURL = "/images/artist.png"
                },
                new Artist()
                {
                Id = 2,
                ArtistName = "Bad Wolfs",
                ArtistPictureURL = "/images/artist.png"
                },
                new Artist()
                {
                Id = 3,
                ArtistName = "Shakira",
                ArtistPictureURL = "/images/artist.png"
                },
                new Artist()
                {
                Id = 4,
                ArtistName = "G-easy",
                ArtistPictureURL = "/images/artist.png"
                }
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Artists.AddRangeAsync(artists);
                await arrangeContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new StatisticsService(assertContext, plMock.Object);
                var result = await sut.FeaturedArtists();

                Assert.IsTrue(result.Count == 3);
            }
        }
    }
}
