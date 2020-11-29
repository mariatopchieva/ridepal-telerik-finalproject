using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using RidePal.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.AdminServiceTests
{
    [TestClass]
    public class UserCount_Should
    {
        [TestMethod]
        public async Task ReturnUserCountOfDb_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnUserCountOfDb_Correctly));
            var plMock = new Mock<IPlaylistService>();

            var users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    UserName = "John Snow",
                    Email = "John@ridepal.com"
                },
                new User()
                {
                    Id = 2,
                    UserName = "Tony Stark",
                    Email = "tony@ridepal.com"
                }
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddRangeAsync(users);
                await arrangeContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new StatisticsService(assertContext, plMock.Object);
                int result = await sut.UserCount();

                Assert.IsTrue(result == 2);
            }
        }
    }
}
