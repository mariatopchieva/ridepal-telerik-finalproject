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
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.PlaylistServiceTests
{
    [TestClass]
    public class AdminGetPlaylistById_Should
    {
        [TestMethod]
        public async Task ReturnCorrectPlaylist_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectPlaylist_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 3,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 4,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            var playlistDTO = new PlaylistDTO
            {
                Id = 4,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
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
                var result = await sut.AdminGetPlaylistByIdAsync(4);

                //Assert
                Assert.AreEqual(playlistDTO.Id, result.Id);
                Assert.AreEqual(playlistDTO.Title, result.Title);
                Assert.AreEqual(playlistDTO.Rank, result.Rank);
                Assert.AreEqual(playlistDTO.UserId, result.UserId);
                Assert.AreEqual(playlistDTO.PlaylistPlaytime, result.PlaylistPlaytime);
            }
        }

        [TestMethod]
        public async Task Throw_If_NoPlaylistsExist()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(Throw_If_NoPlaylistsExist));

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.AdminGetPlaylistByIdAsync(1));
            }
        }
    }
}
