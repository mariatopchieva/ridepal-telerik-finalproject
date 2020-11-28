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
    public class FilterPlaylistsMaster_Should
    {
        [TestMethod]
        public void ReturnOneFilteredPlaylist_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnOneFilteredPlaylist_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 55,
                Title = "Home",
                PlaylistPlaytime = 4824,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 56,
                Title = "Metal",
                PlaylistPlaytime = 5124,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            Playlist thirdPlaylist = new Playlist
            {
                Id = 57,
                Title = "Seaside",
                PlaylistPlaytime = 5324,
                UserId = 2,
                Rank = 552308,
                IsDeleted = false
            };

            Genre rock = new Genre
            {
                Id = 35,
                Name = "rock"
            };

            Genre metal = new Genre
            {
                Id = 36,
                Name = "metal"
            };

            Genre pop = new Genre
            {
                Id = 37,
                Name = "pop"
            };

            Genre jazz = new Genre
            {
                Id = 38,
                Name = "jazz"
            };

            var firstPlaylistGenre = new PlaylistGenre(35, 55);
            var secondPlaylistGenre = new PlaylistGenre(36, 55);
            var thirdPlaylistGenre = new PlaylistGenre(36, 56);
            var fourthPlaylistGenre = new PlaylistGenre(37, 56);
            var fifthPlaylistGenre = new PlaylistGenre(38, 57);

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            var genres = new List<string>() { "metal" };
            List<int> durationLimits = new List<int>() { 0, 84 };
            string name = null;

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Playlists.Add(thirdPlaylist);
                arrangeContext.Genres.Add(metal);
                arrangeContext.Genres.Add(rock);
                arrangeContext.Genres.Add(pop);
                arrangeContext.Genres.Add(jazz);
                arrangeContext.PlaylistGenres.Add(firstPlaylistGenre);
                arrangeContext.PlaylistGenres.Add(secondPlaylistGenre);
                arrangeContext.PlaylistGenres.Add(thirdPlaylistGenre);
                arrangeContext.PlaylistGenres.Add(fourthPlaylistGenre);
                arrangeContext.PlaylistGenres.Add(fifthPlaylistGenre);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
                var result = sut.FilterPlaylistsMasterAsync(name, genres, durationLimits).Result.ToList();

                //Assert
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0].Id, firstPlaylist.Id);
                Assert.AreEqual(result[0].Title, firstPlaylist.Title);
            }
        }
    }
}
