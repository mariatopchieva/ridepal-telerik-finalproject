using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RidePal.Data.Context;
using RidePal.Service;
using RidePal.Service.Providers.Contracts;

namespace RidePal.Services.Tests.GeneratePlaylistServiceTests
{
    [TestClass]
    public class GetPlaytimeString_Should
    {
        [TestMethod]
        public void ReturnCorrectPlaylists_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectPlaylists_WhenParamsAreValid));

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new GeneratePlaylistService(assertContext, dateTimeProviderMock.Object);
                var result = sut.GetPlaytimeString(4856);
                string expected = "1h 20m 56s";

                //Assert
                Assert.AreEqual(result, expected);
            }
        }
    }
}
