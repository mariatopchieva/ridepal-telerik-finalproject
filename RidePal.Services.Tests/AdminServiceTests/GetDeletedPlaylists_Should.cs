using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.AdminServiceTests
{
    [TestClass]
    public class GetDeletedPlaylists_Should
    {
        [TestMethod]
        public async Task GetAllPlaylistsWithDeletedStatus()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(GetAllPlaylistsWithDeletedStatus));
            var playlists = new List<Playlist>()
            {
                new Playlist()
                {
                Id = 1,
                Title = "Good Times",
                IsDeleted = true
                },
                new Playlist()
                {
                Id = 2,
                Title = "Rock Hard",
                IsDeleted = true
                }
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Playlists.AddRangeAsync(playlists);
                await arrangeContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var result = await sut.GetDeletedPlaylists();

                Assert.IsTrue(result.Count == 2);
            }
        }
    }
}
