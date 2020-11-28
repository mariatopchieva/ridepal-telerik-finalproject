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
    public class GetPlaylistsPerPageOfCollection_Should
    {
        [TestMethod]
        public void ReturnTheCorrectPlaylistsPerPageForFavoritePlaylists()
        {
            var options = Utils.GetOptions(nameof(ReturnTheCorrectPlaylistsPerPageForFavoritePlaylists));

            Playlist firstPlaylist = new Playlist
            {
                Id = 75,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 35,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 76,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 35,
                Rank = 490258,
                IsDeleted = false
            };

            Playlist thirdPlaylist = new Playlist
            {
                Id = 77,
                Title = "Jazz",
                PlaylistPlaytime = 5074,
                UserId = 35,
                Rank = 580258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 35
            };

            PlaylistFavorite firstFavorite = new PlaylistFavorite()
            {
                Id = 40,
                UserId = 35,
                PlaylistId = 75,
                IsFavorite = true
            };

            PlaylistFavorite secondFavorite = new PlaylistFavorite()
            {
                Id = 41,
                UserId = 35,
                PlaylistId = 76,
                IsFavorite = true
            };

            PlaylistFavorite thirdFavorite = new PlaylistFavorite()
            {
                Id = 42,
                UserId = 35,
                PlaylistId = 77,
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
                var result = sut.GetPlaylistsPerPageOfCollection(1, 35, "favorites").ToList();

                //Assert
                Assert.AreEqual(result.Count, 3);
            }
        }

        [TestMethod]
        public void ReturnTheCorrectPlaylistsPerPageForUserPlaylists()
        {
            var options = Utils.GetOptions(nameof(ReturnTheCorrectPlaylistsPerPageForUserPlaylists));

            Playlist firstPlaylist = new Playlist
            {
                Id = 78,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 36,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 79,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 36,
                Rank = 490258,
                IsDeleted = false
            };

            Playlist thirdPlaylist = new Playlist
            {
                Id = 80,
                Title = "Jazz",
                PlaylistPlaytime = 5074,
                UserId = 36,
                Rank = 580258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 36
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
                var result = sut.GetPlaylistsPerPageOfCollection(1, 36, "myPlaylists").ToList();

                //Assert
                Assert.AreEqual(result.Count, 3);
            }
        }
    }
}
