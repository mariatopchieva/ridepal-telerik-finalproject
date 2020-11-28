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
    public class UndoDeletePlaylist_Should
    {
        [TestMethod]
        public async Task ReturnTrue_WhenParamsAreValid()
        {
            var options = Utils.GetOptions(nameof(ReturnTrue_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 12,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = true
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 13,
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
                var result = await sut.UndoDeletePlaylistAsync(12);

                //Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_WhenPlaylistIsAlreadyUndeleted()
        {
            var options = Utils.GetOptions(nameof(ReturnFalse_WhenPlaylistIsAlreadyUndeleted));

            Playlist firstPlaylist = new Playlist
            {
                Id = 11,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.UndoDeletePlaylistAsync(11);

                //Assert
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_IfNoPlaylistsExist()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnFalse_IfNoPlaylistsExist));

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.UndoDeletePlaylistAsync(10);

                Assert.IsFalse(result);
            }
        }
    }
}
