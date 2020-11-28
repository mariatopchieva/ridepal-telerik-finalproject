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
    public class GetAllGenres_Should
    {
        [TestMethod]
        public async Task ReturnCorrectGenreCount_WhenParamsAreValid()
        {
            // Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectGenreCount_WhenParamsAreValid));

            Genre rock = new Genre
            {
                Id = 21,
                Name = "rock"
            };

            Genre metal = new Genre
            {
                Id = 22,
                Name = "metal"
            };

            Genre pop = new Genre
            {
                Id = 23,
                Name = "pop"
            };

            Genre jazz = new Genre
            {
                Id = 24,
                Name = "jazz"
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Genres.Add(metal);
                arrangeContext.Genres.Add(rock);
                arrangeContext.Genres.Add(pop);
                arrangeContext.Genres.Add(jazz);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                // Act
                var result = await sut.GetAllGenresAsync();

                //Assert
                Assert.AreEqual(result.Count(), 4);
            }
        }

        [TestMethod]
        public async Task Throw_If_NoGenresExist()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(Throw_If_NoGenresExist));

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.GetAllGenresAsync());
            }
        }
    }
}
