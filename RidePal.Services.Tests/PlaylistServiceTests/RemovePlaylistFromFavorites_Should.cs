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
    public class RemovePlaylistFromFavorites_Should
    {
        [TestMethod]
        public async Task ReturnTrue_WhenPlaylistFavoriteIsRemoved()
        {
            var options = Utils.GetOptions(nameof(ReturnTrue_WhenPlaylistFavoriteIsRemoved));

            Playlist firstPlaylist = new Playlist
            {
                Id = 38,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 8
            };

            PlaylistFavorite favorite = new PlaylistFavorite()
            {
                Id = 5,
                UserId = 8,
                PlaylistId = 38,
                IsFavorite = true
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.Favorites.Add(favorite);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.RemovePlaylistFromFavoritesAsync(38, 8);

                //Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_WhenPlaylistFavoriteWasAlreadyRemoved()
        {
            var options = Utils.GetOptions(nameof(ReturnFalse_WhenPlaylistFavoriteWasAlreadyRemoved));

            Playlist firstPlaylist = new Playlist
            {
                Id = 39,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 9
            };

            PlaylistFavorite favorite = new PlaylistFavorite()
            {
                Id = 6,
                UserId = 9,
                PlaylistId = 39,
                IsFavorite = false
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.Favorites.Add(favorite);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.RemovePlaylistFromFavoritesAsync(39, 9);

                //Assert
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_WhenPlaylistFavoriteDoesNotExist()
        {
            var options = Utils.GetOptions(nameof(ReturnFalse_WhenPlaylistFavoriteDoesNotExist));

            Playlist firstPlaylist = new Playlist
            {
                Id = 40,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 10
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.RemovePlaylistFromFavoritesAsync(40, 10);

                //Assert
                Assert.IsFalse(result);
            }
        }
    }
}
