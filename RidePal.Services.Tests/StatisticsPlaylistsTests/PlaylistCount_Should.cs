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
    public class PlaylistCount_Should
    {
        [TestMethod]
        public async Task ReturnPlaylistCountOfDb_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnPlaylistCountOfDb_Correctly));
            var plMock = new Mock<IPlaylistService>();

            var playlists = new List<Playlist>()
            {
                new Playlist()
                {
                Id = 1,
                Title = "Travel music"
                },
                new Playlist()
                {
                Id = 2,
                Title = "Road Jam"
                }
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Playlists.AddRangeAsync(playlists);
                await arrangeContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new StatisticsService(assertContext, plMock.Object);
                int result = await sut.PlaylistCount();

                Assert.IsTrue(result == 2);
            }
        }
    }
}
