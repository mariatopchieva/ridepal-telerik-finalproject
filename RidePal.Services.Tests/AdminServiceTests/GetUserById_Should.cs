using Microsoft.VisualStudio.TestTools.UnitTesting;
using RidePal.Data.Context;
using RidePal.Data.Models;
using RidePal.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Tests.AdminServiceTests
{
    [TestClass]
    public class GetUserById_Should
    {
        [TestMethod]
        public async Task ReturnUserByHisId_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnUserByHisId_Correctly));

            User user = new User
            {
                Id = 1,
                UserName = "tom",
                Email = "tom@ridepal.com"
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                var userResult = await sut.GetUserById(1);

                Assert.AreEqual(userResult.UserName, user.UserName);
                Assert.AreEqual(userResult.Email, user.Email);
            }
        }

        [TestMethod]
        public async Task ThrowsException_OnNoUserFound()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ThrowsException_OnNoUserFound));

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);

                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => sut.GetUserById(1));
            }
        }
    }
}
