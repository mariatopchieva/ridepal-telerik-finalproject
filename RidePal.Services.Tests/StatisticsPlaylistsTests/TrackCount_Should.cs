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
    public class TrackCount_Should
    {
        [TestMethod]
        public async Task ReturnTrackCountOfDb_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnTrackCountOfDb_Correctly));
            var plMock = new Mock<IPlaylistService>();

            var tracks = new List<Track>()
            {
                new Track()
                {
                Id = 1,
                TrackTitle = "The Hills"
                },
                new Track()
                {
                Id = 2,
                TrackTitle = "Starboy"
                }
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Tracks.AddRangeAsync(tracks);
                await arrangeContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new StatisticsService(assertContext, plMock.Object);
                int result = await sut.TrackCount();

                Assert.IsTrue(result == 2);
            }
        }
    }
}
