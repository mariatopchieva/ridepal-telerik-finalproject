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
    public class TopPlaylists_Should
    {
        [TestMethod]
        public async Task ReturnTopThreePlaylists_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnTopThreePlaylists_Correctly));
            var plMock = new Mock<IPlaylistService>();

            var playlists = new List<Playlist>()
            {
                new Playlist()
                {
                Id = 1,
                Title = "Travel music",
                Rank = 222
                },
                new Playlist()
                {
                Id = 2,
                Title = "Road Jam",
                Rank = 333
                },
                new Playlist()
                {
                Id = 3,
                Title = "Riding Dirty",
                Rank = 444
                },
                new Playlist()
                {
                Id = 4,
                Title = "Heavy metal",
                Rank = 555
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
                var result = await sut.TopPlaylists();

                Assert.IsTrue(result.Count == 3 );
                Assert.IsTrue(result.FirstOrDefault(r => r.Title == "Travel Music") == null);
            }
        }
    }
}
