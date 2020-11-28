using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;

namespace RidePal.Services.Tests.GeneratePlaylistServiceTests
{
    [TestClass]
    public class CalculatePlaytime_Should
    {
        [TestMethod]
        public void ReturnCorrectPlaytime_WhenParamsAreValid()
        {
            // Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectPlaytime_WhenParamsAreValid));

            Track firstTrack = new Track()
            {
                Id = 5,
                ArtistId = 1,
                TrackDuration = 218
            };

            Track secondTrack = new Track()
            {
                Id = 6,
                ArtistId = 1,
                TrackDuration = 257
            };

            var playlist = new List<Track>() { firstTrack, secondTrack };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new GeneratePlaylistService(assertContext, dateTimeProviderMock.Object);
                var result = sut.CalculatePlaytime(playlist);
                var expected = 475.0;

                //Assert
                Assert.AreEqual(result, expected);
            }
        }
    }
}
