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
    public class EditPlaylist_Should
    {
        [TestMethod]
        public async Task ReturnTrue_WhenParamsAreValid()
        {
            // Arrange
            var options = Utils.GetOptions(nameof(ReturnTrue_WhenParamsAreValid));

            Playlist firstPlaylist = new Playlist
            {
                Id = 5,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
                IsDeleted = false,
                
            };

            Playlist secondPlaylist = new Playlist
            {
                Id = 6,
                Title = "Metal",
                PlaylistPlaytime = 5024,
                UserId = 2,
                Rank = 490258,
                IsDeleted = false
            };

            var editPlaylistDTO = new EditPlaylistDTO
            {
                Id = 5,
                Title = "Home and back",
                GenrePercentage = new Dictionary<string, int>()
                {
                    {
                        "rock", 0
                    },
                    {
                        "metal", 0
                    },
                    {
                        "pop", 100
                    },
                    {
                        "jazz", 0
                    }
                },
                UserId = 2
            };

            Genre rock = new Genre
            {
                Id = 1,
                Name = "rock"
            };

            Genre metal = new Genre
            {
                Id = 2,
                Name = "metal"
            };

            Genre pop = new Genre
            {
                Id = 3,
                Name = "pop"
            };

            Genre jazz = new Genre
            {
                Id = 4,
                Name = "jazz"
            };

            var firstPlaylistGenre = new PlaylistGenre(1, 5);
            var secondPlaylistGenre = new PlaylistGenre(2, 5);

            var updatedPlaylistDTO = new PlaylistDTO
            {
                Id = 5,
                Title = "Home and back",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348, 
                GenresCount = 1
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var arrangeContext = new RidePalDbContext(options))
            {
                arrangeContext.Genres.Add(metal);
                arrangeContext.Genres.Add(rock);
                arrangeContext.Genres.Add(pop);
                arrangeContext.Genres.Add(jazz);
                arrangeContext.Playlists.Add(firstPlaylist);
                arrangeContext.Playlists.Add(secondPlaylist);
                arrangeContext.PlaylistGenres.Add(firstPlaylistGenre);
                arrangeContext.PlaylistGenres.Add(secondPlaylistGenre);
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                // Act
                var result = await sut.EditPlaylistAsync(editPlaylistDTO);
                var updatedPlaylistFromDb = await sut.GetPlaylistByIdAsync(5);

                //Assert
                Assert.IsTrue(result);
                Assert.AreEqual(updatedPlaylistDTO.Id, updatedPlaylistFromDb.Id);
                Assert.AreEqual(updatedPlaylistDTO.Title, updatedPlaylistFromDb.Title);
                Assert.AreEqual(updatedPlaylistDTO.GenresCount, updatedPlaylistFromDb.GenresCount);
            }
        }

        [TestMethod]
        public async Task Throw_If_NoPlaylistsExist()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(Throw_If_NoPlaylistsExist));

            var editPlaylistDTO = new EditPlaylistDTO
            {
                Id = 7,
                Title = "Home and back",
                GenrePercentage = new Dictionary<string, int>()
                {
                    {
                        "rock", 0
                    },
                    {
                        "metal", 0
                    },
                    {
                        "pop", 100
                    },
                    {
                        "jazz", 0
                    }
                },
                UserId = 2
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.EditPlaylistAsync(editPlaylistDTO));
            }
        }
    }
}
