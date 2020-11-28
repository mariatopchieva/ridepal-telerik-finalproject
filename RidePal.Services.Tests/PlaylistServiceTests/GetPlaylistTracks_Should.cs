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
using System.Linq;

namespace RidePal.Services.Tests.PlaylistServiceTests
{
    [TestClass]
    public class GetPlaylistTracks_Should
    {
        [TestMethod]
        public void ReturnCorrectPlaylistTracks_WhenParamsAreValid()
        {
            // Arrange
            var options = Utils.GetOptions(nameof(ReturnCorrectPlaylistTracks_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 25,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false,

            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 26,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            Artist artist = new Artist()
            {
                Id = 1,
                ArtistName = "Sade"
            };

            Track firstTrack = new Track()
            {
                Id = 1,
                ArtistId = 1
            };

            Track secondTrack = new Track()
            {
                Id = 2,
                ArtistId = 1
            };

            var firstPlaylistTrack = new PlaylistTrack(1, 25);
            var secondPlaylistTrack = new PlaylistTrack(2, 25);

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.Tracks.Add(firstTrack);
                arrangeContext.Tracks.Add(secondTrack);
                arrangeContext.PlaylistTracks.Add(firstPlaylistTrack);
                arrangeContext.PlaylistTracks.Add(secondPlaylistTrack);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                // Act
                var result = sut.GetPlaylistTracksAsync(25).Result.ToList();

                //Assert
                Assert.AreEqual(result.Count, 2);
                Assert.AreEqual(result[0].ArtistId, artist.Id);
                Assert.AreEqual(result[1].ArtistId, artist.Id);
                Assert.AreEqual(result[0].Id, firstTrack.Id);
                Assert.AreEqual(result[1].Id, secondTrack.Id);
            }
        }

        [TestMethod]
        public async Task Throw_If_NoTracksExist()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(Throw_If_NoTracksExist));

            Playlist firstPlaylist = new Playlist
            {
                Id = 27,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false,

            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.SaveChanges();
            }

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.GetPlaylistTracksAsync(27));
            }
        }
    }
}
