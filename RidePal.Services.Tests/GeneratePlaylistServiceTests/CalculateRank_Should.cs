using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using RidePal.Service.Providers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RidePal.Services.Tests.GeneratePlaylistServiceTests
{
    [TestClass]
    public class CalculateRank_Should
    {
        [TestMethod]
        public void ReturnCorrectRank_WhenParamsAreValid()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectRank_WhenParamsAreValid));

            Track firstTrack = new Track()
            {
                Id = 7,
                ArtistId = 1,
                TrackDuration = 218,
                TrackRank = 587931
            };

            Track secondTrack = new Track()
            {
                Id = 8,
                ArtistId = 1,
                TrackDuration = 257,
                TrackRank = 637965
            };

            var playlist = new List<Track>() { firstTrack, secondTrack };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new GeneratePlaylistService(assertContext, dateTimeProviderMock.Object);
                var result = sut.CalculateRank(playlist);
                var expected = 612948;

                //Assert
                Assert.AreEqual(result, expected);
            }
        }
    }
}
