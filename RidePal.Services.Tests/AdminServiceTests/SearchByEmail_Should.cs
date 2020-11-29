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
    public class SearchByEmail_Should
    {
        [TestMethod]
        public async Task ReturnUserCollection_MatchingTheEmailString()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnUserCollection_MatchingTheEmailString));
            int expected = 2;
            var userEntries = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    UserName = "jenny",
                    Email = "jenny@ridepal.com",
                },
                new User()
                {
                    Id = 2,
                    UserName = "Frank",
                    Email = "Franksemail@ridepal.com",
                },
                 new User()
                {
                    Id = 3,
                    UserName = "Tom",
                    Email = "tomsemail@ridepal.com",
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
                var userResult = await sut.SearchByEmail("email@ridepal.com");
                
                Assert.AreEqual(userResult.Count, expected);
            }
        }

        [TestMethod]
        public async Task ThrowsException_OnNoSearchParamsSubmited()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowsException_OnNoSearchParamsSubmited));
            string searchParams = string.Empty;

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.SearchByEmail(searchParams));
            }
        }
    }
}
