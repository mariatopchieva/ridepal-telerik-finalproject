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
    public class FinetunePlaytime_Should
    {
        [TestMethod]
        public void ReturnPlaylistWithCorrectPlaytime()
        {
            // Arrange
            var options = Utils.GetOptions(nameof(ReturnPlaylistWithCorrectPlaytime));

            Track firstTrack = new Track()
            {
                Id = 7,
                ArtistId = 1,
                TrackDuration = 249
            };

            Track secondTrack = new Track()
            {
                Id = 8,
                ArtistId = 1,
                TrackDuration = 272
            };

            Track thirdTrack = new Track()
            {
                Id = 9,
                ArtistId = 1,
                TrackDuration = 262
            };

            Track fourthTrack = new Track()
            {
                Id = 10,
                ArtistId = 1,
                TrackDuration = 246
            };

            Track fifthTrack = new Track()
            {
                Id = 11,
                ArtistId = 1,
                TrackDuration = 218
            };

            var playlist = new List<Track>() { firstTrack, secondTrack, thirdTrack, fourthTrack, fifthTrack };

            double travelDuration = 900.0;

            var genrePercentage = new Dictionary<string, int>()
            {
                {"rock", 20 },
                {"metal", 20 },
                {"pop", 40 },
                {"jazz", 20 },
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new GeneratePlaylistService(assertContext, dateTimeProviderMock.Object);
                var result = sut.FinetunePlaytime(travelDuration, playlist, genrePercentage).ToList();
                var finalPlaytime = sut.CalculatePlaytime(result);
                var minPlaytime = travelDuration - 300;
                var maxPlaytime = travelDuration + 300;

                //Assert
                Assert.IsTrue(finalPlaytime > minPlaytime);
                Assert.IsTrue(finalPlaytime < maxPlaytime);
            }
        }
    }
}
