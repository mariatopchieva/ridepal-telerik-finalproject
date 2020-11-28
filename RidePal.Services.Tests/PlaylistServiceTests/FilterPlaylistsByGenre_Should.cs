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
    public class FilterPlaylistsByGenre_Should
    {
        [TestMethod]
        public void ReturnTwoPlaylists_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnTwoPlaylists_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 51,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 52,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            Playlist thirdPlaylist = new Playlist
            {
                Id = 53,
                Title = "Seaside",
                PlaylistPlaytime = 5324,
                UserId = 2,
                Rank = 552308,
                IsDeleted = false
            };

            Genre rock = new Genre
            {
                Id = 31,
                Name = "rock"
            };

            Genre metal = new Genre
            {
                Id = 32,
                Name = "metal"
            };

            Genre pop = new Genre
            {
                Id = 33,
                Name = "pop"
            };

            Genre jazz = new Genre
            {
                Id = 34,
                Name = "jazz"
            };

            var firstPlaylistGenre = new PlaylistGenre(31, 51);
            var secondPlaylistGenre = new PlaylistGenre(32, 51);
            var thirdPlaylistGenre = new PlaylistGenre(32, 52);
            var fourthPlaylistGenre = new PlaylistGenre(34, 52);
            var fifthPlaylistGenre = new PlaylistGenre(34, 53);

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            var genres = new List<string>() { "metal"};

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
                var playlists = sut.GetAllPlaylistsAsync().Result;
                var result = sut.FilterPlaylistsByGenreAsync(genres, playlists).Result.ToList();

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
