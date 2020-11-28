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
    public class GetTopTracks
    {
        //[TestMethod]
        //public void ReturnPlaylistWithCorrectPlaytime()
        //{
        //    // Arrange
        //    var options = Utils.GetOptions(nameof(ReturnPlaylistWithCorrectPlaytime));

        //    Genre rock = new Genre()
        //    {
        //        Id = 30,
        //        Name = "rock"
        //    };

        //    Genre pop = new Genre()
        //    {
        //        Id = 31,
        //        Name = "pop"
        //    };

        //    Track firstTrack = new Track()
        //    {
        //        Id = 17,
        //        ArtistId = 1,
        //        TrackDuration = 249,
        //        TrackRank = 512697,
        //        GenreId = 30,
        //        Genre = rock
        //    };

        //    Track secondTrack = new Track()
        //    {
        //        Id = 18,
        //        ArtistId = 1,
        //        TrackDuration = 272,
        //        TrackRank = 457213,
        //        GenreId = 30,
        //        Genre = rock
        //    };

        //    Track thirdTrack = new Track()
        //    {
        //        Id = 19,
        //        ArtistId = 1,
        //        TrackDuration = 262,
        //        TrackRank = 312569,
        //        GenreId = 30,
        //        Genre = rock
        //    };

        //    Track fourthTrack = new Track()
        //    {
        //        Id = 20,
        //        ArtistId = 1,
        //        TrackDuration = 246,
        //        TrackRank = 459632,
        //        GenreId = 30,
        //        Genre = rock
        //    };

        //    Track fifthTrack = new Track()
        //    {
        //        Id = 21,
        //        ArtistId = 1,
        //        TrackDuration = 218,
        //        TrackRank = 542136,
        //        GenreId = 31,
        //        Genre = pop
        //    };

        //    Track sixthTrack = new Track()
        //    {
        //        Id = 22,
        //        ArtistId = 1,
        //        TrackDuration = 252,
        //        TrackRank = 126854,
        //        GenreId = 31,
        //        Genre = pop
        //    };

        //    Track seventhTrack = new Track()
        //    {
        //        Id = 23,
        //        ArtistId = 1,
        //        TrackDuration = 218,
        //        TrackRank = 654236,
        //        GenreId = 31,
        //        Genre = pop
        //    };

        //    Track eightTrack = new Track()
        //    {
        //        Id = 24,
        //        ArtistId = 1,
        //        TrackDuration = 252,
        //        TrackRank = 546259,
        //        GenreId = 31,
        //        Genre = pop
        //    };

        //    using (var arrangeContext = new RidePalDbContext(options))
        //    {
        //        arrangeContext.Genres.Add(rock);
        //        arrangeContext.Genres.Add(pop);
        //        arrangeContext.Tracks.Add(firstTrack);
        //        arrangeContext.Tracks.Add(secondTrack);
        //        arrangeContext.Tracks.Add(thirdTrack);
        //        arrangeContext.Tracks.Add(fourthTrack);
        //        arrangeContext.Tracks.Add(fifthTrack);
        //        arrangeContext.Tracks.Add(sixthTrack);
        //        arrangeContext.Tracks.Add(seventhTrack);
        //        arrangeContext.Tracks.Add(eightTrack);
        //        arrangeContext.SaveChanges();
        //    }

        //    double travelDuration = 660.0;

        //    var genrePercentage = new Dictionary<string, int>()
        //    {
        //        {"rock", 50 },
        //        {"metal", 0 },
        //        {"pop", 50 },
        //        {"jazz", 0 },
        //    };

        //    var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        //    string genre = "pop";

        //    using (var assertContext = new RidePalDbContext(options))
        //    {
        //        //Act
        //        var sut = new GeneratePlaylistService(assertContext, dateTimeProviderMock.Object);
        //        var result = sut.GetTracks(genre, travelDuration, genrePercentage, true).Result.ToList();

        //        var finalPlaytime = sut.CalculatePlaytime(result);

        //        //Assert
        //        Assert.IsTrue(finalPlaytime >= travelDuration / 2);
        //        Assert.IsTrue(result.Count > 1);
        //    }
        //}
    }
}
