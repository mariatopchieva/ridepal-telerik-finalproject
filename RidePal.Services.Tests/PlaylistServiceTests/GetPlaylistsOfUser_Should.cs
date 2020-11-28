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
    public class GetPlaylistsOfUser_Should
    {
        [TestMethod]
        public void ReturnCorrectPlaylists_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectPlaylists_WhenParamsAreValid));

            var firstUser = new User();
            firstUser.Id = 1;
            var secondUser = new User();
            secondUser.Id = 2;

            Playlist firstPlaylist = new Playlist
            {
                Id = 14,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 1,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 15,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            var firstPlaylistDTO = new PlaylistDTO
            {
                Id = 14,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 1,
                Rank = 552348
            };

            var secondPlaylistDTO = new PlaylistDTO
            {
                Id = 15,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Users.Add(firstUser);
                arrangeContext.Users.Add(secondUser);
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = sut.GetPlaylistsOfUserAsync(1).Result.ToList();

                //Assert
                Assert.AreEqual(result[0].Id, firstPlaylistDTO.Id);
                Assert.AreEqual(result[0].Title, firstPlaylistDTO.Title);
                Assert.AreEqual(result.Count, 1);
            }
        }

        [TestMethod]
        public async Task Throw_If_UserDoesNotExist()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(Throw_If_UserDoesNotExist));

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.GetPlaylistsOfUserAsync(1));
            }
        }
    }
}
