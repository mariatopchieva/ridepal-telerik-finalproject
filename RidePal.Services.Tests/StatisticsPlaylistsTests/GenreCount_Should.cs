using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using RidePal.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.AdminServiceTests
{
    [TestClass]
    public class GenreCount_Should
    {
        [TestMethod]
        public async Task ReturnGenreCountOfDb_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnGenreCountOfDb_Correctly));
            var plMock = new Mock<IPlaylistService>();

            var genres = new List<Genre>()
            {
                new Genre()
                {
                Id = 1,
                Name = "Metal"
                },
                new Genre()
                {
                Id = 2,
                Name = "Rock"
                }
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Genres.AddRangeAsync(genres);
                await arrangeContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new StatisticsService(assertContext, plMock.Object);
                int result = await sut.GenreCount();

                Assert.IsTrue(result == 2);
            }
        }
    }
}
