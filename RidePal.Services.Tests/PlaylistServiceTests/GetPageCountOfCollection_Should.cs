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

namespace RidePal.Services.Tests.PlaylistServiceTests
{
    [TestClass]
    public class GetPageCountOfCollection_Should
    {
        [TestMethod]
        public void ReturnTheCorrectPageCountOfFavoritePlaylists()
        {
            var options = Utils.GetOptions(nameof(ReturnTheCorrectPageCountOfFavoritePlaylists));

            Playlist firstPlaylist = new Playlist
            {
                Id = 63,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 30,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 64,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 30,
                Rank = 490258,
                IsDeleted = false
            };

            Playlist thirdPlaylist = new Playlist
            {
                Id = 65,
                Title = "Jazz",
                PlaylistPlaytime = 5074,
                UserId = 30,
                Rank = 580258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 30
            };

            PlaylistFavorite firstFavorite = new PlaylistFavorite()
            {
                Id = 30,
                UserId = 30,
                PlaylistId = 63,
                IsFavorite = true
            };

            PlaylistFavorite secondFavorite = new PlaylistFavorite()
            {
                Id = 31,
                UserId = 30,
                PlaylistId = 64,
                IsFavorite = true
            };

            PlaylistFavorite thirdFavorite = new PlaylistFavorite()
            {
                Id = 32,
                UserId = 30,
                PlaylistId = 65,
                IsFavorite = true
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Playlists.Add(thirdPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.Favorites.Add(firstFavorite);
                arrangeContext.Favorites.Add(secondFavorite);
                arrangeContext.Favorites.Add(thirdFavorite);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = sut.GetPageCountOfCollection(30, "favorites");

                //Assert
                Assert.AreEqual(result, 1);
            }
        }

        [TestMethod]
        public void ReturnTheCorrectPageCountOfUserPlaylists()
        {
            var options = Utils.GetOptions(nameof(ReturnTheCorrectPageCountOfUserPlaylists));

            Playlist firstPlaylist = new Playlist
            {
                Id = 66,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 31,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 67,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 31,
                Rank = 490258,
                IsDeleted = false
            };

            Playlist thirdPlaylist = new Playlist
            {
                Id = 68,
                Title = "Jazz",
                PlaylistPlaytime = 5074,
                UserId = 31,
                Rank = 580258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 31
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Playlists.Add(thirdPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = sut.GetPageCountOfCollection(31, "myPlaylists");

                //Assert
                Assert.AreEqual(result, 1);
            }
        }
    }
}
