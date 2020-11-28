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
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.PlaylistServiceTests
{
    [TestClass]
    public class GetPlaylistGenresAsString_Should
    {
        [TestMethod]
        public async Task ReturnCorrectString_WhenParamsAreValid()
        {
            // Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectString_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 17,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false,

            };

            Genre rock = new Genre
            {
                Id = 9,
                Name = "rock"
            };

            Genre metal = new Genre
            {
                Id = 10,
                Name = "metal"
            };

            Genre pop = new Genre
            {
                Id = 11,
                Name = "pop"
            };

            Genre jazz = new Genre
            {
                Id = 12,
                Name = "jazz"
            };

            var firstPlaylistGenre = new PlaylistGenre(9, 17);
            var secondPlaylistGenre = new PlaylistGenre(10, 17);

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Genres.Add(metal);
                arrangeContext.Genres.Add(rock);
                arrangeContext.Genres.Add(pop);
                arrangeContext.Genres.Add(jazz);
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.PlaylistGenres.Add(firstPlaylistGenre);
                arrangeContext.PlaylistGenres.Add(secondPlaylistGenre);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                // Act
                var result = await sut.GetPlaylistGenresAsStringAsync(17);
                string expectedString = "metal, rock";

                //Assert
                Assert.AreEqual(result, expectedString);
            }
        }

        [TestMethod]
        public async Task ThrowWhenNoGenresAreFound()
        {
            // Arrange
            var options = Utils.GetOptions(nameof(ThrowWhenNoGenresAreFound));

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.GetPlaylistGenresAsStringAsync(17));
            }
        }
    }
}
