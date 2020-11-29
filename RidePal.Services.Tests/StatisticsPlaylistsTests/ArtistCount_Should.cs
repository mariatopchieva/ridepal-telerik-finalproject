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
    public class ArtistCount_Should
    {
        [TestMethod]
        public async Task ReturnArtistCountOfDb_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnArtistCountOfDb_Correctly));
            var plMock = new Mock<IPlaylistService>();

            var artists = new List<Artist>()
            {
                new Artist()
                {
                Id = 1,
                ArtistName = "The weeknd"
                },
                new Artist()
                {
                Id = 2,
                ArtistName = "Bad Wolfs"
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
                int result = await sut.ArtistCount();

                Assert.IsTrue(result == 2);
            }
        }
    }
}
