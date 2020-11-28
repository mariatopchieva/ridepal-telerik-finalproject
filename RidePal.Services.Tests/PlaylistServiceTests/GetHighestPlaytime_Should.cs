using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using RidePal.Service.Contracts;
using RidePal.Service.DTO;
using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.PlaylistServiceTests
{
    [TestClass]
    public class GetHighestPlaytime_Should
    {
        [TestMethod]
        public async Task ReturnCorrectNumber_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectNumber_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 16,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 17,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.GetHighestPlaytimeAsync();

                //Assert
                Assert.AreEqual(93, result);
            }
        }

        [TestMethod]
        public async Task ReturnZero_WhenThereAreNoPlaylists()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnZero_WhenThereAreNoPlaylists));

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.GetHighestPlaytimeAsync();

                //Assert
                Assert.AreEqual(0, result);
            }
        }
    }
}
