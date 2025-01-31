﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class GetFavoritePlaylistsOfUser_Should
    {
        [TestMethod]
        public void ReturnTheCorrectPlaylists_WhenParamsAreValid()
        {
            var options = Utils.GetOptions(nameof(ReturnTheCorrectPlaylists_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 41,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 42,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            User user = new User()
            {
                Id = 11
            };

            PlaylistFavorite firstFavorite = new PlaylistFavorite()
            {
                Id = 10,
                UserId = 11,
                PlaylistId = 41,
                IsFavorite = true
            };

            PlaylistFavorite secondFavorite = new PlaylistFavorite()
            {
                Id = 11,
                UserId = 11,
                PlaylistId = 42,
                IsFavorite = true
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Users.Add(user);
                arrangeContext.Favorites.Add(firstFavorite);
                arrangeContext.Favorites.Add(secondFavorite);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = sut.GetFavoritePlaylistsOfUser(11).Result.ToList();

                //Assert
                Assert.AreEqual(result.Count, 2);
                Assert.AreEqual(result[0].Id, firstPlaylist.Id);
                Assert.AreEqual(result[0].Title, firstPlaylist.Title);
                Assert.AreEqual(result[1].Id, secondPlaylist.Id);
                Assert.AreEqual(result[1].Title, secondPlaylist.Title);
            }
        }
    }
}
