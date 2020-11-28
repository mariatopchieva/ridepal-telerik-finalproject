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
    public class FilterPlaylistsByName_Should
    {
        [TestMethod]
        public void ReturnOnePlaylist_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnOnePlaylist_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 45,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 46,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            Playlist thirdPlaylist = new Playlist
            {
                Id = 47,
                Title = "Seaside",
                PlaylistPlaytime = 5324,
                UserId = 2,
                Rank = 552308,
                IsDeleted = false
            };

            var firstPlaylistDTO = new PlaylistDTO
            {
                Id = 45,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Playlists.Add(thirdPlaylist);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var playlists = sut.GetAllPlaylistsAsync().Result;
                var result = sut.FilterPlaylistsByName("Home", playlists).ToList();

                //Assert
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0].Id, firstPlaylistDTO.Id);
                Assert.AreEqual(result[0].Title, firstPlaylistDTO.Title);
            }
        }
    }
}
