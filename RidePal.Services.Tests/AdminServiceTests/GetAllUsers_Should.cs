using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using RidePal.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.AdminServiceTests
{
    [TestClass]
    public class GetAllUsers_Should
    {
        [TestMethod]
        public async Task ReturnAllUsers_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnAllUsers_Correctly));
            var userEntries = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    UserName = "admin",
                    Email = "admin@ridepal.com"
                },
                new User()
                {
                    Id = 2,
                    UserName = "user",
                    Email = "user@ridepal.com"
                }
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddRangeAsync(userEntries);
                await arrangeContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var userResult = await sut.GetAllUsers();

                Assert.AreEqual(userResult.Count, userEntries.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsEmptyCollection_OnNoUsersFound()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnsEmptyCollection_OnNoUsersFound));

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var result = await sut.GetAllUsers();

                Assert.IsTrue(result.Count == 0);
            }
        }
    }
}
