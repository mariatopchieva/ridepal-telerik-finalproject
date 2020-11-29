using Microsoft.EntityFrameworkCore;
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
    public class RevertDeleteUser_Should
    {
        [TestMethod]
        public async Task RevertDeletedStatus_Correctly()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(RevertDeletedStatus_Correctly));

            User user = new User
            {
                Id = 1,
                UserName = "tom",
                Email = "tom@ridepal.com",
                IsDeleted = true
            };

            //Act
            using (var arrangeContext = new RidePalDbContext(options))
            {
                await arrangeContext.Users.AddAsync(user);
                await arrangeContext.SaveChangesAsync();

                var sut = new AdminService(arrangeContext);
                bool delResult = await sut.RevertDeleteUser(1);
            }

            //Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var userFromDb = await assertContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

                Assert.IsFalse(userFromDb.IsDeleted);
            }
        }

        [TestMethod]
        public async Task ReturnFalse_OnUserInDbNotFound()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(ReturnFalse_OnUserInDbNotFound));

            //Act & Assert
            using (var assertContext = new RidePalDbContext(options))
            {
                var sut = new AdminService(assertContext);
                bool result = await sut.RevertDeleteUser(1);

                Assert.IsFalse(result);
            }
        }
    }
}
