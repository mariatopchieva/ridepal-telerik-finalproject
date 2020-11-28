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
    public class AttachImage_Should
    {
        [TestMethod]
        public async Task Throw_When_NoPlaylistsExistToAttachImageTo()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(Throw_When_NoPlaylistsExistToAttachImageTo));

            PlaylistDTO firstPlaylistDTO = new PlaylistDTO
            {
                Id = 91,
                Title = "Home",
                PlaylistPlaytime = 5524,
                UserId = 2,
                Rank = 552348,
            };

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var mockImageService = new Mock<IPixaBayImageService>();

            using (var assertContext = new RidePalDbContext(options))
            {
                //Act
                var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);

                //Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.AttachImage(firstPlaylistDTO));
            }


            //[TestMethod]
            //public async Task ReturnPlaylistWithFilePath_WhenParamsAreValid()
            //{
            //    //Arrange
            //    var options = Utils.GetOptions(nameof(ReturnPlaylistWithFilePath_WhenParamsAreValid));

            //    Playlist firstPlaylist = new Playlist
            //    {
            //        Id = 87,
            //        Title = "Home",
            //        PlaylistPlaytime = 5524,
            //        UserId = 2,
            //        Rank = 552348,
            //        IsDeleted = false
            //    };

            //    PlaylistDTO firstPlaylistDTO = new PlaylistDTO
            //    {
            //        Id = 87,
            //        Title = "Home",
            //        PlaylistPlaytime = 5524,
            //        UserId = 2,
            //        Rank = 552348,
            //    };

            //    var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            //    var mockImageService = new Mock<IPixaBayImageService>();
            //    string name = "name";

            //    //mockImageService
            //    //    .Setup(x => x.AssignImage(It.IsAny<PlaylistDTO>()))
            //    //    .Returns(name); //cannot convert from string to System.ThreadingTasks.Task<string>

            //    using (var arrangeContext = new RidePalDbContext(options))
            //    {
            //        arrangeContext.Playlists.Add(firstPlaylist);
            //        arrangeContext.SaveChanges();
            //    }

            //    using (var assertContext = new RidePalDbContext(options))
            //    {
            //        //Act
            //        var sut = new PlaylistService(assertContext, dateTimeProviderMock.Object, mockImageService.Object);
            //        var result = await sut.AttachImage(firstPlaylistDTO);

            //        //Assert
            //        Assert.IsNotNull(result.FilePath);
            //    }
            //}
        }
    }
}
