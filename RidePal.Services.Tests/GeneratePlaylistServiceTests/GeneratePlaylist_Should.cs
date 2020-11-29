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

namespace RidePal.Services.Tests.GeneratePlaylistServiceTests
{
    [TestClass]
    public class GeneratePlaylist_Should
    {
        //[TestMethod]
        //public async Task ReturnCorrectPlaylistDTO_WhenParamsAreValid()
        //{
        //    // Arrange
        //    var options = Utils.GetOptions(nameof(ReturnCorrectPlaylistDTO_WhenParamsAreValid));

        //    Genre rock = new Genre()
        //    {
        //        Id = 40,
        //        Name = "rock"
        //    };

        //    Genre pop = new Genre()
        //    {
        //        Id = 41,
        //        Name = "pop"
        //    };

        //    User user = new User()
        //    {
        //        Id = 45,
        //    };

        //    Track firstTrack = new Track()
        //    {
        //        Id = 1200,
        //        ArtistId = 1,
        //        TrackDuration = 249,
        //        GenreId = 40,
        //        Genre = rock
        //    };

        //    Track secondTrack = new Track()
        //    {
        //        Id = 1201,
        //        ArtistId = 1,
        //        TrackDuration = 272,
        //        GenreId = 40,
        //        Genre = rock
        //    };

        //    Track thirdTrack = new Track()
        //    {
        //        Id = 1202,
        //        ArtistId = 1,
        //        TrackDuration = 262,
        //        GenreId = 40,
        //        Genre = rock
        //    };

        //    Track fourthTrack = new Track()
        //    {
        //        Id = 1203,
        //        ArtistId = 1,
        //        TrackDuration = 246,
        //        GenreId = 40,
        //        Genre = rock
        //    };

        //    Track fifthTrack = new Track()
        //    {
        //        Id = 1204,
        //        ArtistId = 1,
        //        TrackDuration = 289,
        //        GenreId = 40,
        //        Genre = rock
        //    };

        //    Track sixthTrack = new Track()
        //    {
        //        Id = 1205,
        //        ArtistId = 1,
        //        TrackDuration = 272,
        //        GenreId = 40,
        //        Genre = rock
        //    };

        //    Track seventhTrack = new Track()
        //    {
        //        Id = 1206,
        //        ArtistId = 1,
        //        TrackDuration = 266,
        //        GenreId = 40,
        //        Genre = rock
        //    };

        //    Track eightTrack = new Track()
        //    {
        //        Id = 1207,
        //        ArtistId = 1,
        //        TrackDuration = 218,
        //        GenreId = 41,
        //        Genre = pop
        //    };

        //    Track ninthTrack = new Track()
        //    {
        //        Id = 1208,
        //        ArtistId = 1,
        //        TrackDuration = 252,
        //        GenreId = 41,
        //        Genre = pop
        //    };

        //    Track tenthTrack = new Track()
        //    {
        //        Id = 1209,
        //        ArtistId = 1,
        //        TrackDuration = 218,
        //        GenreId = 41,
        //        Genre = pop
        //    };

        //    Track eleventhTrack = new Track()
        //    {
        //        Id = 1210,
        //        ArtistId = 1,
        //        TrackDuration = 252,
        //        GenreId = 41,
        //        Genre = pop
        //    };

        //    Track twelfthTrack = new Track()
        //    {
        //        Id = 1211,
        //        ArtistId = 1,
        //        TrackDuration = 275,
        //        GenreId = 41,
        //        Genre = pop
        //    };

        //    Track thirteenthTrack = new Track()
        //    {
        //        Id = 1212,
        //        ArtistId = 1,
        //        TrackDuration = 220,
        //        GenreId = 41,
        //        Genre = pop
        //    };

        //    Track fourteenthTrack = new Track()
        //    {
        //        Id = 1213,
        //        ArtistId = 1,
        //        TrackDuration = 262,
        //        GenreId = 41,
        //        Genre = pop
        //    };

        //    using (var arrangeContext = new RidePalDbContext(options))
        //    {
        //        arrangeContext.Genres.Add(rock);
        //        arrangeContext.Genres.Add(pop);
        //        arrangeContext.Users.Add(user);
        //        arrangeContext.Tracks.Add(firstTrack);
        //        arrangeContext.Tracks.Add(secondTrack);
        //        arrangeContext.Tracks.Add(thirdTrack);
        //        arrangeContext.Tracks.Add(fourthTrack);
        //        arrangeContext.Tracks.Add(fifthTrack);
        //        arrangeContext.Tracks.Add(sixthTrack);
        //        arrangeContext.Tracks.Add(seventhTrack);
        //        arrangeContext.Tracks.Add(eightTrack);
        //        arrangeContext.Tracks.Add(ninthTrack);
        //        arrangeContext.Tracks.Add(tenthTrack);
        //        arrangeContext.Tracks.Add(eleventhTrack);
        //        arrangeContext.Tracks.Add(twelfthTrack);
        //        arrangeContext.Tracks.Add(thirteenthTrack);
        //        arrangeContext.Tracks.Add(fourteenthTrack);
        //        arrangeContext.SaveChanges();
        //    }

        //    var genrePercentage = new Dictionary<string, int>()
        //    {
        //        {"rock", 50 },
        //        {"metal", 0 },
        //        {"pop", 50 },
        //        {"jazz", 0 },
        //    };

        //    GeneratePlaylistDTO genPlaylistDTO = new GeneratePlaylistDTO()
        //    {
        //        StartLocation = "Plovdiv",
        //        Destination = "Parvenets",
        //        PlaylistName = "Bike around",
        //        RepeatArtist = true,
        //        UseTopTracks = false,
        //        GenrePercentage = genrePercentage,
        //        User = user,
        //        UserId = 45
        //    };

        //    var dateTimeProviderMock = new Mock<IDateTimeProvider>();

        //    using (var assertContext = new RidePalDbContext(options))
        //    {
        //        //Act
        //        var sut = new GeneratePlaylistService(assertContext, dateTimeProviderMock.Object);
        //        var result = await sut.GeneratePlaylist(genPlaylistDTO);

        //        //Assert
        //        Assert.AreEqual(result.Title, genPlaylistDTO.PlaylistName);
        //        Assert.AreEqual(result.UserId, genPlaylistDTO.UserId);
        //        Assert.AreEqual(result.GenresCount, 2);

        //    }
        }
    }
}
