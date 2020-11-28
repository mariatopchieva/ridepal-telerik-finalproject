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
    public class AddPlaylistToFavorites_Should
    {
        [TestMethod]
        public async Task ReturnTrue_WhenNewPlaylistFavoriteIsCreated()
        {
            var options = Utils.GetOptions(nameof(ReturnTrue_WhenNewPlaylistFavoriteIsCreated));

            Playlist firstPlaylist = new Playlist
            {
                Id = 30,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 31,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 3
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.AddPlaylistToFavoritesAsync(30, 3);

                //Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task ReturnTrue_WhenPlaylistFavoriteWasCreatedButDisliked()
        {
            var options = Utils.GetOptions(nameof(ReturnTrue_WhenPlaylistFavoriteWasCreatedButDisliked));

            Playlist firstPlaylist = new Playlist
            {
                Id = 32,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 33,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 4
            };

            PlaylistFavorite favorite = new PlaylistFavorite() 
            { 
                Id = 2,
                UserId = 4,
                PlaylistId = 32,
                IsFavorite = false
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.Favorites.Add(favorite);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.AddPlaylistToFavoritesAsync(32, 4);

                //Assert
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_WhenPlaylistFavoriteWasCreatedAndAlreadyLiked()
        {
            var options = Utils.GetOptions(nameof(ReturnFalse_WhenPlaylistFavoriteWasCreatedAndAlreadyLiked));

            Playlist firstPlaylist = new Playlist
            {
                Id = 34,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 35,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 5
            };

            PlaylistFavorite favorite = new PlaylistFavorite()
            {
                Id = 2,
                UserId = 5,
                PlaylistId = 34,
                IsFavorite = true
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.Favorites.Add(favorite);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.AddPlaylistToFavoritesAsync(34, 5);

                //Assert
                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task ReturnTrue_WhenPlaylistFavoriteIsCreated()
        {
            var options = Utils.GetOptions(nameof(ReturnTrue_WhenPlaylistFavoriteIsCreated));

            Playlist firstPlaylist = new Playlist
            {
                Id = 36,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 37,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 6
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = await sut.AddPlaylistToFavoritesAsync(36, 6);

                //Assert
                Assert.IsTrue(result);
            }
        }
    }
}
